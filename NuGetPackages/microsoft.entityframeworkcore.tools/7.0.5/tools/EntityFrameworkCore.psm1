$ErrorActionPreference = 'Stop'

#
# Add-Migration
#

Register-TabExpansion Add-Migration @{
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Adds a new migration.

.DESCRIPTION
    Adds a new migration.

.PARAMETER Name
    The name of the migration.

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory. Defaults to "Migrations".

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Remove-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Add-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [string] $Name,
        [string] $OutputDir,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Namespace,
        [string] $Args)

    WarnIfEF6 'Add-Migration'

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'add', $Name, '--json'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($Namespace)
    {
        $params += '--namespace', $Namespace
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json
    Write-Host 'To undo this action, use Remove-Migration.'

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
       $dteProject.ProjectItems.AddFromFile($result.migrationFile) | Out-Null

       $dteProject.ProjectItems.AddFromFile($result.metadataFile) | Out-Null

       $dteProject.ProjectItems.AddFromFile($result.snapshotFile) | Out-Null
    }

    $DTE.ItemOperations.OpenFile($result.migrationFile) | Out-Null
    ShowConsole
}

#
# Bundle-Migration
#

Register-TabExpansion Bundle-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Creates an executable to update the database.

.DESCRIPTION
    Creates an executable to update the database.

.PARAMETER Output
    The path of executable file to create.

.PARAMETER Force
    Overwrite existing files.

.PARAMETER SelfContained
    Also bundle the .NET runtime so it doesn't need to be installed on the machine.

.PARAMETER TargetRuntime
    The target runtime to bundle for.

.PARAMETER Framework
    The target framework. Defaults to the first one in the project.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Script-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Bundle-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Output,
        [switch] $Force,
        [switch] $SelfContained,
        [string] $TargetRuntime,
        [string] $Framework,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Framework)
    {
        $Framework = GetProperty $dteStartupProject.Properties 'FriendlyTargetFramework'
    }

    $params = 'migrations', 'bundle', '--framework', $Framework

    if ($Output)
    {
        $params += '--output', $Output
    }

    if ($Force)
    {
        $params += '--force'
    }

    if ($SelfContained)
    {
        $params += '--self-contained'
    }

    if ($TargetRuntime)
    {
        $params += '--target-runtime', $TargetRuntime
    }

    $params += GetParams $Context

    EF $dteProject $dteStartupProject $params $Args
}

#
# Drop-Database
#

Register-TabExpansion Drop-Database @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Drops the database.

.DESCRIPTION
    Drops the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Update-Database
    about_EntityFrameworkCore
#>
function Drop-Database
{
    [CmdletBinding(PositionalBinding = $false, SupportsShouldProcess = $true, ConfirmImpact = 'High')]
    param(
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $info = Get-DbContext -Context $Context -Project $Project -StartupProject $StartupProject

    if ($PSCmdlet.ShouldProcess("database '$($info.databaseName)' on server '$($info.dataSource)'"))
    {
        $params = 'database', 'drop', '--force'
        $params += GetParams $Context

        EF $dteProject $dteStartupProject $params $Args -skipBuild
    }
}

#
# Enable-Migrations (Obsolete)
#

function Enable-Migrations
{
    WarnIfEF6 'Enable-Migrations'
    Write-Warning 'Enable-Migrations is obsolete. Use Add-Migration to start using Migrations.'
}

#
# Get-DbContext
#

Register-TabExpansion Get-DbContext @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Lists and gets information about available DbContext types.

.DESCRIPTION
    Lists and gets information about available DbContext types.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Get-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if ($PSBoundParameters.ContainsKey('Context'))
    {
       $params = 'dbcontext', 'info', '--json'
       $params += GetParams $Context

       # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
       return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json
    }
    else
    {
       $params = 'dbcontext', 'list', '--json'
       # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
       # NB: Write-Output is here to break the JSON array into individual items
       return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json | Write-Output | Format-Table -Property safeName -HideTableHeaders
    }
}

#
# Get-Migration
#

Register-TabExpansion Get-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Lists available migrations.

.DESCRIPTION
    Lists available migrations.

.PARAMETER Connection
    The connection string to the database. Defaults to the one specified in AddDbContext or OnConfiguring.

.PARAMETER NoConnect
    Don't connect to the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Add-Migration
    Remove-Migration
    Update-Database
    about_EntityFrameworkCore
#>
function Get-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Connection,
        [switch] $NoConnect,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'list', '--json'

    if ($Connection)
    {
        $params += '--connection', $Connection
    }

    if ($NoConnect)
    {
        $params += '--no-connect'
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    return (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json | Write-Output
}

#
# Remove-Migration
#

Register-TabExpansion Remove-Migration @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Removes the last migration.

.DESCRIPTION
    Removes the last migration.

.PARAMETER Force
    Revert the migration if it has been applied to the database.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Add-Migration
    Get-Migration
    about_EntityFrameworkCore
#>
function Remove-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [switch] $Force,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'migrations', 'remove', '--json'

    if ($Force)
    {
        $params += '--force'
    }

    $params += GetParams $Context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
        $files = $result.migrationFile, $result.metadataFile, $result.snapshotFile
        $files | ?{ $_ -ne $null } | %{
            $projectItem = GetProjectItem $dteProject $_
            if ($projectItem)
            {
                $projectItem.Remove()
            }
        }
    }
}

#
# Optimize-DbContext
#

Register-TabExpansion Optimize-DbContext @{
    Project = { GetProjects }
    StartupProject = { GetProjects }
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
}

<#
.SYNOPSIS
    Generates a compiled version of the model used by the DbContext.

.DESCRIPTION
    Generates a compiled version of the model used by the DbContext.

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Optimize-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $OutputDir,
        [string] $Namespace,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'dbcontext', 'optimize'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($Namespace)
    {
        $params += '--namespace', $Namespace
    }

    if ($Context)
    {
        $params += '--context', $Context
    }

    EF $dteProject $dteStartupProject $params $Args
}

#
# Scaffold-DbContext
#

Register-TabExpansion Scaffold-DbContext @{
    Provider = { param($x) GetProviders $x.Project }
    Project = { GetProjects }
    StartupProject = { GetProjects }
    OutputDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
    ContextDir = { <# Disabled. Otherwise, paths would be relative to the solution directory. #> }
}

<#
.SYNOPSIS
    Scaffolds a DbContext and entity types for a database.

.DESCRIPTION
    Scaffolds a DbContext and entity types for a database.

.PARAMETER Connection
    The connection string to the database.

.PARAMETER Provider
    The provider to use. (E.g. Microsoft.EntityFrameworkCore.SqlServer)

.PARAMETER OutputDir
    The directory to put files in. Paths are relative to the project directory.

.PARAMETER ContextDir
    The directory to put the DbContext file in. Paths are relative to the project directory.

.PARAMETER Context
    The name of the DbContext. Defaults to the database name.

.PARAMETER Schemas
    The schemas of tables to generate entity types for.

.PARAMETER Tables
    The tables to generate entity types for.

.PARAMETER DataAnnotations
    Use attributes to configure the model (where possible). If omitted, only the fluent API is used.

.PARAMETER UseDatabaseNames
    Use table and column names directly from the database.

.PARAMETER Force
    Overwrite existing files.

.PARAMETER NoOnConfiguring
    Don't generate DbContext.OnConfiguring.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Namespace
    The namespace to use. Matches the directory by default.

.PARAMETER ContextNamespace
    The namespace of the DbContext class. Matches the directory by default.

.PARAMETER NoPluralize
    Don't use the pluralizer.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Scaffold-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0, Mandatory = $true)]
        [string] $Connection,
        [Parameter(Position = 1, Mandatory = $true)]
        [string] $Provider,
        [string] $OutputDir,
        [string] $ContextDir,
        [string] $Context,
        [string[]] $Schemas = @(),
        [string[]] $Tables = @(),
        [switch] $DataAnnotations,
        [switch] $UseDatabaseNames,
        [switch] $Force,
        [switch] $NoOnConfiguring,
        [string] $Project,
        [string] $StartupProject,
        [string] $Namespace,
        [string] $ContextNamespace,
        [switch] $NoPluralize,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'dbcontext', 'scaffold', $Connection, $Provider, '--json'

    if ($OutputDir)
    {
        $params += '--output-dir', $OutputDir
    }

    if ($ContextDir)
    {
        $params += '--context-dir', $ContextDir
    }

    if ($Context)
    {
        $params += '--context', $Context
    }

    if ($PSBoundParameters.ContainsKey('Namespace'))
    {
        $params += '--namespace', $Namespace
    }

    if ($PSBoundParameters.ContainsKey('ContextNamespace'))
    {
        $params += '--context-namespace', $ContextNamespace
    }

    $params += $Schemas | %{ '--schema', $_ }
    $params += $Tables | %{ '--table', $_ }

    if ($DataAnnotations)
    {
        $params += '--data-annotations'
    }

    if ($UseDatabaseNames)
    {
        $params += '--use-database-names'
    }

    if ($Force)
    {
        $params += '--force'
    }

    if ($NoOnConfiguring)
    {
        $params += '--no-onconfiguring'
    }

    if ($NoPluralize)
    {
        $params += '--no-pluralize'
    }

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    $result = (EF $dteProject $dteStartupProject $params $Args) -join "`n" | ConvertFrom-Json

    if (!(IsCpsProject $dteProject) -or (GetCpsProperty $dteProject 'EnableDefaultItems') -ne 'true' -or (GetCpsProperty $dteProject 'EnableDefaultCompileItems') -ne 'true')
    {
       $files = $result.entityTypeFiles + $result.contextFile
       $files | %{ $dteProject.ProjectItems.AddFromFile($_) | Out-Null }
    }

    $DTE.ItemOperations.OpenFile($result.contextFile) | Out-Null
    ShowConsole
}

#
# Script-DbContext
#

Register-TabExpansion Script-DbContext @{
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Generates a SQL script from the DbContext. Bypasses any migrations.

.DESCRIPTION
    Generates a SQL script from the DbContext. Bypasses any migrations.

.PARAMETER Output
    The file to write the result to.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    about_EntityFrameworkCore
#>
function Script-DbContext
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [string] $Output,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Output)
    {
        $intermediatePath = GetIntermediatePath $dteProject
        if (![IO.Path]::IsPathRooted($intermediatePath))
        {
            $projectDir = GetProperty $dteProject.Properties 'FullPath'
            $intermediatePath = [IO.Path]::GetFullPath((Join-Path $projectDir $intermediatePath))
        }

        $scriptFileName = [IO.Path]::ChangeExtension([IO.Path]::GetRandomFileName(), '.sql')
        $Output = Join-Path $intermediatePath $scriptFileName
    }
    elseif (![IO.Path]::IsPathRooted($Output))
    {
        $Output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Output)
    }

    $params = 'dbcontext', 'script', '--output', $Output
    $params += GetParams $Context

    EF $dteProject $dteStartupProject $params $Args

    $DTE.ItemOperations.OpenFile($Output) | Out-Null
    ShowConsole
}

#
# Script-Migration
#

Register-TabExpansion Script-Migration @{
    From = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    To = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Generates a SQL script from migrations.

.DESCRIPTION
    Generates a SQL script from migrations.

.PARAMETER From
    The starting migration. Defaults to '0' (the initial database).

.PARAMETER To
    The target migration. Defaults to the last migration.

.PARAMETER Idempotent
    Generate a script that can be used on a database at any migration.

.PARAMETER NoTransactions
    Don't generate SQL transaction statements.

.PARAMETER Output
    The file to write the result to.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Update-Database
    Get-Migration
    about_EntityFrameworkCore
#>
function Script-Migration
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(ParameterSetName = 'WithoutTo', Position = 0)]
        [Parameter(ParameterSetName = 'WithTo', Position = 0, Mandatory = $true)]
        [string] $From,
        [Parameter(ParameterSetName = 'WithTo', Position = 1, Mandatory = $true)]
        [string] $To,
        [switch] $Idempotent,
        [switch] $NoTransactions,
        [string] $Output,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    if (!$Output)
    {
        $intermediatePath = GetIntermediatePath $dteProject
        if (![IO.Path]::IsPathRooted($intermediatePath))
        {
            $projectDir = GetProperty $dteProject.Properties 'FullPath'
            $intermediatePath = [IO.Path]::GetFullPath((Join-Path $projectDir $intermediatePath))
        }

        $scriptFileName = [IO.Path]::ChangeExtension([IO.Path]::GetRandomFileName(), '.sql')
        $Output = Join-Path $intermediatePath $scriptFileName
    }
    elseif (![IO.Path]::IsPathRooted($Output))
    {
        $Output = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Output)
    }

    $params = 'migrations', 'script', '--output', $Output

    if ($From)
    {
        $params += $From
    }

    if ($To)
    {
        $params += $To
    }

    if ($Idempotent)
    {
        $params += '--idempotent'
    }

    if ($NoTransactions)
    {
        $params += '--no-transactions'
    }

    $params += GetParams $Context


    EF $dteProject $dteStartupProject $params $Args

    $DTE.ItemOperations.OpenFile($Output) | Out-Null
    ShowConsole
}

#
# Update-Database
#

Register-TabExpansion Update-Database @{
    Migration = { param($x) GetMigrations $x.Context $x.Project $x.StartupProject }
    Context = { param($x) GetContextTypes $x.Project $x.StartupProject }
    Project = { GetProjects }
    StartupProject = { GetProjects }
}

<#
.SYNOPSIS
    Updates the database to a specified migration.

.DESCRIPTION
    Updates the database to a specified migration.

.PARAMETER Migration
    The target migration. If '0', all migrations will be reverted. Defaults to the last migration.

.PARAMETER Connection
    The connection string to the database. Defaults to the one specified in AddDbContext or OnConfiguring.

.PARAMETER Context
    The DbContext to use.

.PARAMETER Project
    The project to use.

.PARAMETER StartupProject
    The startup project to use. Defaults to the solution's startup project.

.PARAMETER Args
    Arguments passed to the application.

.LINK
    Script-Migration
    about_EntityFrameworkCore
#>
function Update-Database
{
    [CmdletBinding(PositionalBinding = $false)]
    param(
        [Parameter(Position = 0)]
        [string] $Migration,
        [string] $Connection,
        [string] $Context,
        [string] $Project,
        [string] $StartupProject,
        [string] $Args)

    WarnIfEF6 'Update-Database'

    $dteProject = GetProject $Project
    $dteStartupProject = GetStartupProject $StartupProject $dteProject

    $params = 'database', 'update'

    if ($Migration)
    {
        $params += $Migration
    }

    if ($Connection)
    {
        $params += '--connection', $Connection
    }

    $params += GetParams $Context


    EF $dteProject $dteStartupProject $params $Args
}

#
# (Private Helpers)
#

function GetProjects
{
    return Get-Project -All | %{ $_.ProjectName }
}

function GetProviders($projectName)
{
    if (!$projectName)
    {
        $projectName = (Get-Project).ProjectName
    }

    return Get-Package -ProjectName $projectName | %{ $_.Id }
}

function GetContextTypes($projectName, $startupProjectName)
{
    $project = GetProject $projectName
    $startupProject = GetStartupProject $startupProjectName $project

    $params = 'dbcontext', 'list', '--json'

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    $result = (EF $project $startupProject $params $null -skipBuild) -join "`n" | ConvertFrom-Json | Write-Output

    return $result | %{ $_.safeName }
}

function GetMigrations($context, $projectName, $startupProjectName)
{
    $project = GetProject $projectName
    $startupProject = GetStartupProject $startupProjectName $project

    $params = 'migrations', 'list', '--no-connect', '--json'
    $params += GetParams $context

    # NB: -join is here to support ConvertFrom-Json on PowerShell 3.0
    # NB: Write-Output is here to break the JSON array into individual items
    $result = (EF $project $startupProject $params $null -skipBuild) -join "`n" | ConvertFrom-Json | Write-Output

    return $result | %{ $_.safeName }
}

function WarnIfEF6($cmdlet)
{
    if (Get-Module 'EntityFramework6')
    {
        Write-Warning "Both Entity Framework Core and Entity Framework 6 are installed. The Entity Framework Core tools are running. Use 'EntityFramework6\$cmdlet' for Entity Framework 6."
    }
    elseif (Get-Module 'EntityFramework')
    {
        Write-Warning "Both Entity Framework Core and Entity Framework 6 are installed. The Entity Framework Core tools are running. Use 'EntityFramework\$cmdlet' for Entity Framework 6."
    }
}

function GetProject($projectName)
{
    if (!$projectName)
    {
        return Get-Project
    }

    return Get-Project $projectName
}

function GetStartupProject($name, $fallbackProject)
{
    if ($name)
    {
        return Get-Project $name
    }

    $startupProjectPaths = $DTE.Solution.SolutionBuild.StartupProjects
    if ($startupProjectPaths)
    {
        if ($startupProjectPaths.Length -eq 1)
        {
            $startupProjectPath = $startupProjectPaths[0]
            if (![IO.Path]::IsPathRooted($startupProjectPath))
            {
                $solutionPath = Split-Path (GetProperty $DTE.Solution.Properties 'Path')
                $startupProjectPath = [IO.Path]::GetFullPath((Join-Path $solutionPath $startupProjectPath))
            }

            $startupProject = GetSolutionProjects | ?{
                try
                {
                    $fullName = $_.FullName
                }
                catch [NotImplementedException]
                {
                    return $false
                }

                if ($fullName -and $fullName.EndsWith('\'))
                {
                    $fullName = $fullName.Substring(0, $fullName.Length - 1)
                }

                return $fullName -eq $startupProjectPath
            }
            if ($startupProject)
            {
                return $startupProject
            }

            Write-Warning "Unable to resolve startup project '$startupProjectPath'."
        }
        else
        {
            Write-Warning 'Multiple startup projects set.'
        }
    }
    else
    {
        Write-Warning 'No startup project set.'
    }

    Write-Warning "Using project '$($fallbackProject.ProjectName)' as the startup project."

    return $fallbackProject
}

function GetSolutionProjects()
{
    $projects = New-Object 'System.Collections.Stack'

    $DTE.Solution.Projects | %{
        $projects.Push($_)
    }

    while ($projects.Count)
    {
        $project = $projects.Pop();

        <# yield return #> $project

        if ($project.ProjectItems)
        {
            $project.ProjectItems | ?{ $_.SubProject } | %{
                $projects.Push($_.SubProject)
            }
        }
    }
}

function GetParams($context)
{
    $params = @()

    if ($context)
    {
        $params += '--context', $context
    }

    return $params
}

function ShowConsole
{
    $componentModel = Get-VSComponentModel
    $powerConsoleWindow = $componentModel.GetService([NuGetConsole.IPowerConsoleWindow])
    $powerConsoleWindow.Show()
}

function WriteErrorLine($message)
{
    try
    {
        # Call the internal API NuGet uses to display errors
        $componentModel = Get-VSComponentModel
        $powerConsoleWindow = $componentModel.GetService([NuGetConsole.IPowerConsoleWindow])
        $bindingFlags = [Reflection.BindingFlags]::Instance -bor [Reflection.BindingFlags]::NonPublic
        $activeHostInfo = $powerConsoleWindow.GetType().GetProperty('ActiveHostInfo', $bindingFlags).GetValue($powerConsoleWindow)
        $internalHost = $activeHostInfo.WpfConsole.Host
        $reportErrorMethod = $internalHost.GetType().GetMethod('ReportError', $bindingFlags, $null, [Exception], $null)
        $exception = New-Object Exception $message
        $reportErrorMethod.Invoke($internalHost, $exception)
    }
    catch
    {
        Write-Host $message -ForegroundColor DarkRed
    }
}

function EF($project, $startupProject, $params, $applicationArgs, [switch] $skipBuild)
{
    if (IsDocker $startupProject)
    {
        throw "Startup project '$($startupProject.ProjectName)' is a Docker project. Select an ASP.NET Core Web " +
            'Application as your startup project and try again.'
    }
    if (IsUWP $startupProject)
    {
        throw "Startup project '$($startupProject.ProjectName)' is a Universal Windows Platform app. This version of " +
            'the Entity Framework Core Package Manager Console Tools doesn''t support this type of project. For more ' +
            'information on using the EF Core Tools with UWP projects, see ' +
            'https://go.microsoft.com/fwlink/?linkid=858496'
    }

    Write-Verbose "Using project '$($project.ProjectName)'."
    Write-Verbose "Using startup project '$($startupProject.ProjectName)'."

    if (!$skipBuild)
    {
        Write-Host 'Build started...'

        # TODO: Only build startup project. Don't use BuildProject, you can't specify platform
        $solutionBuild = $DTE.Solution.SolutionBuild
        $solutionBuild.Build(<# WaitForBuildToFinish: #> $true)
        if ($solutionBuild.LastBuildInfo)
        {
            throw 'Build failed.'
        }

        Write-Host 'Build succeeded.'
    }

    $activeConfiguration = $startupProject.ConfigurationManager.ActiveConfiguration
    if ($activeConfiguration -eq $null)
    {
        throw "Unable to read project configuration settings of project '$($startupProject.ProjectName)' for the " +
            'active solution configuration. Try closing Package Manager Console and restarting Visual Studio. If the ' +
            'problem persists, use Help > Send Feedback > Report a Problem.'
    }

    $startupProjectDir = GetProperty $startupProject.Properties 'FullPath'
    $outputPath = GetProperty $activeConfiguration.Properties 'OutputPath'
    $targetDir = [IO.Path]::GetFullPath([IO.Path]::Combine($startupProjectDir, $outputPath))
    $startupTargetFileName = GetProperty $startupProject.Properties 'OutputFileName'
    $startupTargetPath = Join-Path $targetDir $startupTargetFileName
    $targetFrameworkMoniker = GetProperty $startupProject.Properties 'TargetFrameworkMoniker'
    $frameworkName = New-Object 'System.Runtime.Versioning.FrameworkName' $targetFrameworkMoniker
    $targetFramework = $frameworkName.Identifier

    if ($targetFramework -in '.NETFramework')
    {
        $platformTarget = GetPlatformTarget $startupProject
        if ($platformTarget -eq 'x86')
        {
            $exePath = Join-Path $PSScriptRoot 'net461\win-x86\ef.exe'
        }
        elseif ($platformTarget -eq 'ARM64')
        {
            $exePath = Join-Path $PSScriptRoot 'net461\win-arm64\ef.exe'
        }
        elseif ($platformTarget -in 'AnyCPU', 'x64')
        {
            $exePath = Join-Path $PSScriptRoot 'net461\any\ef.exe'
        }
        else
        {
            throw "Startup project '$($startupProject.ProjectName)' has an active platform of '$platformTarget'. Select " +
                'a different platform and try again.'
        }
    }
    elseif ($targetFramework -eq '.NETCoreApp')
    {
        $targetPlatformIdentifier = GetCpsProperty $startupProject 'TargetPlatformIdentifier'
        if ($targetPlatformIdentifier -and $targetPlatformIdentifier -ne 'Windows')
        {
            throw "Startup project '$($startupProject.ProjectName)' targets platform '$targetPlatformIdentifier'. The Entity Framework " +
                'Core Package Manager Console Tools don''t support this platform. See https://aka.ms/efcore-docs-pmc-tfms for more ' +
                'information.'
        }

        $exePath = (Get-Command 'dotnet').Path

        $startupTargetName = GetProperty $startupProject.Properties 'AssemblyName'
        $depsFile = Join-Path $targetDir ($startupTargetName + '.deps.json')
        $projectAssetsFile = GetCpsProperty $startupProject 'ProjectAssetsFile'
        $runtimeConfig = Join-Path $targetDir ($startupTargetName + '.runtimeconfig.json')
        $runtimeFrameworkVersion = GetCpsProperty $startupProject 'RuntimeFrameworkVersion'
        $efPath = Join-Path $PSScriptRoot 'netcoreapp2.0\any\ef.dll'

        $dotnetParams = 'exec', '--depsfile', $depsFile

        if ($projectAssetsFile)
        {
            # NB: Don't use Get-Content. It doesn't handle UTF-8 without a signature
            # NB: Don't use ReadAllLines. ConvertFrom-Json won't work on PowerShell 3.0
            $projectAssets = [IO.File]::ReadAllText($projectAssetsFile) | ConvertFrom-Json
            $projectAssets.packageFolders.psobject.Properties.Name | %{
                $dotnetParams += '--additionalprobingpath', $_.TrimEnd('\')
            }
        }

        if ([IO.File]::Exists($runtimeConfig))
        {
            $dotnetParams += '--runtimeconfig', $runtimeConfig
        }
        elseif ($runtimeFrameworkVersion)
        {
            $dotnetParams += '--fx-version', $runtimeFrameworkVersion
        }

        $dotnetParams += $efPath

        $params = $dotnetParams + $params
    }
    elseif ($targetFramework -eq '.NETStandard')
    {
        throw "Startup project '$($startupProject.ProjectName)' targets framework '.NETStandard'. There is no " +
            'runtime associated with this framework, and projects targeting it cannot be executed directly. To use ' +
            'the Entity Framework Core Package Manager Console Tools with this project, add an executable project ' +
            'targeting .NET Framework or .NET Core that references this project, and set it as the startup project; ' +
            'or, update this project to cross-target .NET Framework or .NET Core. For more information on using the ' +
            'EF Core Tools with .NET Standard projects, see https://go.microsoft.com/fwlink/?linkid=2034705'
    }
    else
    {
        throw "Startup project '$($startupProject.ProjectName)' targets framework '$targetFramework'. The Entity Framework Core Package " +
            'Manager Console Tools don''t support this framework. See https://aka.ms/efcore-docs-pmc-tfms for more information and examples.'
    }

    $projectDir = GetProperty $project.Properties 'FullPath'
    $targetFileName = GetProperty $project.Properties 'OutputFileName'
    $targetPath = Join-Path $targetDir $targetFileName
    $rootNamespace = GetProperty $project.Properties 'RootNamespace'
    $language = GetLanguage $project
    $nullable = GetNullable $project

    $params += '--verbose',
        '--no-color',
        '--prefix-output',
        '--assembly', $targetPath,
        '--project', $project.FullName,
        '--startup-assembly', $startupTargetPath,
        '--startup-project', $startupProject.FullName,
        '--project-dir', $projectDir,
        '--language', $language,
        '--configuration', $activeConfiguration.ConfigurationName,
        '--working-dir', $PWD.Path

    if (IsWeb $startupProject)
    {
        $params += '--data-dir', (Join-Path $startupProjectDir 'App_Data')
    }

    if ($rootNamespace -ne $null)
    {
        $params += '--root-namespace', $rootNamespace
    }

    if ($nullable -in 'enable', 'annotations')
    {
        $params += '--nullable'
    }

    $arguments = ToArguments $params
    if ($applicationArgs)
    {
        $arguments += ' -- '
        $arguments += $applicationArgs
    }

    $startInfo = New-Object 'System.Diagnostics.ProcessStartInfo' -Property @{
        FileName = $exePath;
        Arguments = $arguments;
        UseShellExecute = $false;
        CreateNoWindow = $true;
        RedirectStandardOutput = $true;
        StandardOutputEncoding = [Text.Encoding]::UTF8;
        RedirectStandardError = $true;
        WorkingDirectory = $startupProjectDir;
    }

    Write-Verbose "$exePath $arguments"

    $process = [Diagnostics.Process]::Start($startInfo)

    while (($line = $process.StandardOutput.ReadLine()) -ne $null)
    {
        $level = $null
        $text = $null

        $parts = $line.Split(':', 2)
        if ($parts.Length -eq 2)
        {
            $level = $parts[0]

            $i = 0
            $count = 8 - $level.Length
            while ($i -lt $count -and $parts[1][$i] -eq ' ')
            {
                $i++
            }

            $text = $parts[1].Substring($i)
        }

        switch ($level)
        {
            'error' { WriteErrorLine $text }
            'warn' { Write-Warning $text }
            'info' { Write-Host $text }
            'data' { Write-Output $text }
            'verbose' { Write-Verbose $text }
            default { Write-Host $line }
        }
    }

    $process.WaitForExit()

    if ($process.ExitCode)
    {
        while (($line = $process.StandardError.ReadLine()) -ne $null)
        {
            WriteErrorLine $line
        }

        exit
    }
}

function IsDocker($project)
{
    return $project.Kind -eq '{E53339B2-1760-4266-BCC7-CA923CBCF16C}'
}

function IsCpsProject($project)
{
    $hierarchy = GetVsHierarchy $project
    $isCapabilityMatch = [Microsoft.VisualStudio.Shell.PackageUtilities].GetMethod(
        'IsCapabilityMatch',
        [type[]]([Microsoft.VisualStudio.Shell.Interop.IVsHierarchy], [string]))

    return $isCapabilityMatch.Invoke($null, ($hierarchy, 'CPS'))
}

function IsWeb($project)
{
    $types = GetProjectTypes $project

    return $types -contains '{349C5851-65DF-11DA-9384-00065B846F21}'
}

function IsUWP($project)
{
    $types = GetProjectTypes $project

    return $types -contains '{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}'
}

function GetIntermediatePath($project)
{
    $intermediatePath = GetProperty $project.ConfigurationManager.ActiveConfiguration.Properties 'IntermediatePath'
    if ($intermediatePath)
    {
        return $intermediatePath
    }

    return GetMSBuildProperty $project 'IntermediateOutputPath'
}

function GetPlatformTarget($project)
{
    if (IsCpsProject $project)
    {
        $platformTarget = GetCpsProperty $project 'PlatformTarget'
        if ($platformTarget)
        {
            return $platformTarget
        }

        return GetCpsProperty $project 'Platform'
    }

    $platformTarget = GetProperty $project.ConfigurationManager.ActiveConfiguration.Properties 'PlatformTarget'
    if ($platformTarget)
    {
        return $platformTarget
    }

    # NB: For classic F# projects
    $platformTarget = GetMSBuildProperty $project 'PlatformTarget'
    if ($platformTarget)
    {
        return $platformTarget
    }

    return 'AnyCPU'
}

function GetLanguage($project)
{
    if (IsCpsProject $project)
    {
        return GetCpsProperty $project 'Language'
    }

    return GetMSBuildProperty $project 'Language'
}

function GetNullable($project)
{
    if (IsCpsProject $project)
    {
        return GetCpsProperty $project 'Nullable'
    }

    return GetMSBuildProperty $project 'Nullable'
}

function GetVsHierarchy($project)
{
    $solution = Get-VSService 'Microsoft.VisualStudio.Shell.Interop.SVsSolution' 'Microsoft.VisualStudio.Shell.Interop.IVsSolution'
    $hierarchy = $null
    $hr = $solution.GetProjectOfUniqueName($project.UniqueName, [ref] $hierarchy)
    [Runtime.InteropServices.Marshal]::ThrowExceptionForHR($hr)

    return $hierarchy
}

function GetProjectTypes($project)
{
    $hierarchy = GetVsHierarchy $project
    $aggregatableProject = Get-Interface $hierarchy 'Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject'
    if (!$aggregatableProject)
    {
        return $project.Kind
    }

    $projectTypeGuidsString = $null
    $hr = $aggregatableProject.GetAggregateProjectTypeGuids([ref] $projectTypeGuidsString)
    [Runtime.InteropServices.Marshal]::ThrowExceptionForHR($hr)

    return $projectTypeGuidsString.Split(';')
}

function GetProperty($properties, $propertyName)
{
    try
    {
        return $properties.Item($propertyName).Value
    }
    catch
    {
        return $null
    }
}

function GetCpsProperty($project, $propertyName)
{
    $browseObjectContext = Get-Interface $project 'Microsoft.VisualStudio.ProjectSystem.Properties.IVsBrowseObjectContext'
    $unconfiguredProject = $browseObjectContext.UnconfiguredProject
    $configuredProject = $unconfiguredProject.GetSuggestedConfiguredProjectAsync().Result
    $properties = $configuredProject.Services.ProjectPropertiesProvider.GetCommonProperties()

    return $properties.GetEvaluatedPropertyValueAsync($propertyName).Result
}

function GetMSBuildProperty($project, $propertyName)
{
    $msbuildProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.LoadedProjects |
        ? FullPath -eq $project.FullName

    return $msbuildProject.GetProperty($propertyName).EvaluatedValue
}

function GetProjectItem($project, $path)
{
    $fullPath = GetProperty $project.Properties 'FullPath'

    if ([IO.Path]::IsPathRooted($path))
    {
        $path = $path.Substring($fullPath.Length)
    }

    $itemDirectory = (Split-Path $path -Parent)

    $projectItems = $project.ProjectItems
    if ($itemDirectory)
    {
        $directories = $itemDirectory.Split('\')
        $directories | %{
            if ($projectItems)
            {
                $projectItems = $projectItems.Item($_).ProjectItems
            }
        }
    }

    if (!$projectItems)
    {
        return $null
    }

    $itemName = Split-Path $path -Leaf

    try
    {
        return $projectItems.Item($itemName)
    }
    catch [Exception]
    {
    }

    return $null
}

function ToArguments($params)
{
    $arguments = ''
    for ($i = 0; $i -lt $params.Length; $i++)
    {
        if ($i)
        {
            $arguments += ' '
        }

        if ($params[$i] -eq '')
        {
            $arguments += '""'

            continue
        }
        if (!$params[$i].Contains(' '))
        {
            $arguments += $params[$i]

            continue
        }

        $arguments += '"'

        $pendingBackslashs = 0
        for ($j = 0; $j -lt $params[$i].Length; $j++)
        {
            switch ($params[$i][$j])
            {
                '"'
                {
                    if ($pendingBackslashs)
                    {
                        $arguments += '\' * $pendingBackslashs * 2
                        $pendingBackslashs = 0
                    }
                    $arguments += '\"'
                }

                '\'
                {
                    $pendingBackslashs++
                }

                default
                {
                    if ($pendingBackslashs)
                    {
                        if ($pendingBackslashs -eq 1)
                        {
                            $arguments += '\'
                        }
                        else
                        {
                            $arguments += '\' * $pendingBackslashs * 2
                        }

                        $pendingBackslashs = 0
                    }

                    $arguments += $params[$i][$j]
                }
            }
        }

        if ($pendingBackslashs)
        {
            $arguments += '\' * $pendingBackslashs * 2
        }

        $arguments += '"'
    }

    return $arguments
}

# SIG # Begin signature block
# MIInpQYJKoZIhvcNAQcCoIInljCCJ5ICAQExDzANBglghkgBZQMEAgEFADB5Bgor
# BgEEAYI3AgEEoGswaTA0BgorBgEEAYI3AgEeMCYCAwEAAAQQH8w7YFlLCE63JNLG
# KX7zUQIBAAIBAAIBAAIBAAIBADAxMA0GCWCGSAFlAwQCAQUABCBRC55CYtrLxWYG
# sANHaMgRQYHL5Z36WYzCBXy+kn2yuaCCDYUwggYDMIID66ADAgECAhMzAAACzfNk
# v/jUTF1RAAAAAALNMA0GCSqGSIb3DQEBCwUAMH4xCzAJBgNVBAYTAlVTMRMwEQYD
# VQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01pY3Jvc29mdCBDb2RlIFNpZ25p
# bmcgUENBIDIwMTEwHhcNMjIwNTEyMjA0NjAyWhcNMjMwNTExMjA0NjAyWjB0MQsw
# CQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9u
# ZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMR4wHAYDVQQDExVNaWNy
# b3NvZnQgQ29ycG9yYXRpb24wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIB
# AQDrIzsY62MmKrzergm7Ucnu+DuSHdgzRZVCIGi9CalFrhwtiK+3FIDzlOYbs/zz
# HwuLC3hir55wVgHoaC4liQwQ60wVyR17EZPa4BQ28C5ARlxqftdp3H8RrXWbVyvQ
# aUnBQVZM73XDyGV1oUPZGHGWtgdqtBUd60VjnFPICSf8pnFiit6hvSxH5IVWI0iO
# nfqdXYoPWUtVUMmVqW1yBX0NtbQlSHIU6hlPvo9/uqKvkjFUFA2LbC9AWQbJmH+1
# uM0l4nDSKfCqccvdI5l3zjEk9yUSUmh1IQhDFn+5SL2JmnCF0jZEZ4f5HE7ykDP+
# oiA3Q+fhKCseg+0aEHi+DRPZAgMBAAGjggGCMIIBfjAfBgNVHSUEGDAWBgorBgEE
# AYI3TAgBBggrBgEFBQcDAzAdBgNVHQ4EFgQU0WymH4CP7s1+yQktEwbcLQuR9Zww
# VAYDVR0RBE0wS6RJMEcxLTArBgNVBAsTJE1pY3Jvc29mdCBJcmVsYW5kIE9wZXJh
# dGlvbnMgTGltaXRlZDEWMBQGA1UEBRMNMjMwMDEyKzQ3MDUzMDAfBgNVHSMEGDAW
# gBRIbmTlUAXTgqoXNzcitW2oynUClTBUBgNVHR8ETTBLMEmgR6BFhkNodHRwOi8v
# d3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2NybC9NaWNDb2RTaWdQQ0EyMDExXzIw
# MTEtMDctMDguY3JsMGEGCCsGAQUFBwEBBFUwUzBRBggrBgEFBQcwAoZFaHR0cDov
# L3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9jZXJ0cy9NaWNDb2RTaWdQQ0EyMDEx
# XzIwMTEtMDctMDguY3J0MAwGA1UdEwEB/wQCMAAwDQYJKoZIhvcNAQELBQADggIB
# AE7LSuuNObCBWYuttxJAgilXJ92GpyV/fTiyXHZ/9LbzXs/MfKnPwRydlmA2ak0r
# GWLDFh89zAWHFI8t9JLwpd/VRoVE3+WyzTIskdbBnHbf1yjo/+0tpHlnroFJdcDS
# MIsH+T7z3ClY+6WnjSTetpg1Y/pLOLXZpZjYeXQiFwo9G5lzUcSd8YVQNPQAGICl
# 2JRSaCNlzAdIFCF5PNKoXbJtEqDcPZ8oDrM9KdO7TqUE5VqeBe6DggY1sZYnQD+/
# LWlz5D0wCriNgGQ/TWWexMwwnEqlIwfkIcNFxo0QND/6Ya9DTAUykk2SKGSPt0kL
# tHxNEn2GJvcNtfohVY/b0tuyF05eXE3cdtYZbeGoU1xQixPZAlTdtLmeFNly82uB
# VbybAZ4Ut18F//UrugVQ9UUdK1uYmc+2SdRQQCccKwXGOuYgZ1ULW2u5PyfWxzo4
# BR++53OB/tZXQpz4OkgBZeqs9YaYLFfKRlQHVtmQghFHzB5v/WFonxDVlvPxy2go
# a0u9Z+ZlIpvooZRvm6OtXxdAjMBcWBAsnBRr/Oj5s356EDdf2l/sLwLFYE61t+ME
# iNYdy0pXL6gN3DxTVf2qjJxXFkFfjjTisndudHsguEMk8mEtnvwo9fOSKT6oRHhM
# 9sZ4HTg/TTMjUljmN3mBYWAWI5ExdC1inuog0xrKmOWVMIIHejCCBWKgAwIBAgIK
# YQ6Q0gAAAAAAAzANBgkqhkiG9w0BAQsFADCBiDELMAkGA1UEBhMCVVMxEzARBgNV
# BAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jv
# c29mdCBDb3Jwb3JhdGlvbjEyMDAGA1UEAxMpTWljcm9zb2Z0IFJvb3QgQ2VydGlm
# aWNhdGUgQXV0aG9yaXR5IDIwMTEwHhcNMTEwNzA4MjA1OTA5WhcNMjYwNzA4MjEw
# OTA5WjB+MQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UE
# BxMHUmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSgwJgYD
# VQQDEx9NaWNyb3NvZnQgQ29kZSBTaWduaW5nIFBDQSAyMDExMIICIjANBgkqhkiG
# 9w0BAQEFAAOCAg8AMIICCgKCAgEAq/D6chAcLq3YbqqCEE00uvK2WCGfQhsqa+la
# UKq4BjgaBEm6f8MMHt03a8YS2AvwOMKZBrDIOdUBFDFC04kNeWSHfpRgJGyvnkmc
# 6Whe0t+bU7IKLMOv2akrrnoJr9eWWcpgGgXpZnboMlImEi/nqwhQz7NEt13YxC4D
# dato88tt8zpcoRb0RrrgOGSsbmQ1eKagYw8t00CT+OPeBw3VXHmlSSnnDb6gE3e+
# lD3v++MrWhAfTVYoonpy4BI6t0le2O3tQ5GD2Xuye4Yb2T6xjF3oiU+EGvKhL1nk
# kDstrjNYxbc+/jLTswM9sbKvkjh+0p2ALPVOVpEhNSXDOW5kf1O6nA+tGSOEy/S6
# A4aN91/w0FK/jJSHvMAhdCVfGCi2zCcoOCWYOUo2z3yxkq4cI6epZuxhH2rhKEmd
# X4jiJV3TIUs+UsS1Vz8kA/DRelsv1SPjcF0PUUZ3s/gA4bysAoJf28AVs70b1FVL
# 5zmhD+kjSbwYuER8ReTBw3J64HLnJN+/RpnF78IcV9uDjexNSTCnq47f7Fufr/zd
# sGbiwZeBe+3W7UvnSSmnEyimp31ngOaKYnhfsi+E11ecXL93KCjx7W3DKI8sj0A3
# T8HhhUSJxAlMxdSlQy90lfdu+HggWCwTXWCVmj5PM4TasIgX3p5O9JawvEagbJjS
# 4NaIjAsCAwEAAaOCAe0wggHpMBAGCSsGAQQBgjcVAQQDAgEAMB0GA1UdDgQWBBRI
# bmTlUAXTgqoXNzcitW2oynUClTAZBgkrBgEEAYI3FAIEDB4KAFMAdQBiAEMAQTAL
# BgNVHQ8EBAMCAYYwDwYDVR0TAQH/BAUwAwEB/zAfBgNVHSMEGDAWgBRyLToCMZBD
# uRQFTuHqp8cx0SOJNDBaBgNVHR8EUzBRME+gTaBLhklodHRwOi8vY3JsLm1pY3Jv
# c29mdC5jb20vcGtpL2NybC9wcm9kdWN0cy9NaWNSb29DZXJBdXQyMDExXzIwMTFf
# MDNfMjIuY3JsMF4GCCsGAQUFBwEBBFIwUDBOBggrBgEFBQcwAoZCaHR0cDovL3d3
# dy5taWNyb3NvZnQuY29tL3BraS9jZXJ0cy9NaWNSb29DZXJBdXQyMDExXzIwMTFf
# MDNfMjIuY3J0MIGfBgNVHSAEgZcwgZQwgZEGCSsGAQQBgjcuAzCBgzA/BggrBgEF
# BQcCARYzaHR0cDovL3d3dy5taWNyb3NvZnQuY29tL3BraW9wcy9kb2NzL3ByaW1h
# cnljcHMuaHRtMEAGCCsGAQUFBwICMDQeMiAdAEwAZQBnAGEAbABfAHAAbwBsAGkA
# YwB5AF8AcwB0AGEAdABlAG0AZQBuAHQALiAdMA0GCSqGSIb3DQEBCwUAA4ICAQBn
# 8oalmOBUeRou09h0ZyKbC5YR4WOSmUKWfdJ5DJDBZV8uLD74w3LRbYP+vj/oCso7
# v0epo/Np22O/IjWll11lhJB9i0ZQVdgMknzSGksc8zxCi1LQsP1r4z4HLimb5j0b
# pdS1HXeUOeLpZMlEPXh6I/MTfaaQdION9MsmAkYqwooQu6SpBQyb7Wj6aC6VoCo/
# KmtYSWMfCWluWpiW5IP0wI/zRive/DvQvTXvbiWu5a8n7dDd8w6vmSiXmE0OPQvy
# CInWH8MyGOLwxS3OW560STkKxgrCxq2u5bLZ2xWIUUVYODJxJxp/sfQn+N4sOiBp
# mLJZiWhub6e3dMNABQamASooPoI/E01mC8CzTfXhj38cbxV9Rad25UAqZaPDXVJi
# hsMdYzaXht/a8/jyFqGaJ+HNpZfQ7l1jQeNbB5yHPgZ3BtEGsXUfFL5hYbXw3MYb
# BL7fQccOKO7eZS/sl/ahXJbYANahRr1Z85elCUtIEJmAH9AAKcWxm6U/RXceNcbS
# oqKfenoi+kiVH6v7RyOA9Z74v2u3S5fi63V4GuzqN5l5GEv/1rMjaHXmr/r8i+sL
# gOppO6/8MO0ETI7f33VtY5E90Z1WTk+/gFcioXgRMiF670EKsT/7qMykXcGhiJtX
# cVZOSEXAQsmbdlsKgEhr/Xmfwb1tbWrJUnMTDXpQzTGCGXYwghlyAgEBMIGVMH4x
# CzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRt
# b25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xKDAmBgNVBAMTH01p
# Y3Jvc29mdCBDb2RlIFNpZ25pbmcgUENBIDIwMTECEzMAAALN82S/+NRMXVEAAAAA
# As0wDQYJYIZIAWUDBAIBBQCgga4wGQYJKoZIhvcNAQkDMQwGCisGAQQBgjcCAQQw
# HAYKKwYBBAGCNwIBCzEOMAwGCisGAQQBgjcCARUwLwYJKoZIhvcNAQkEMSIEIMYL
# aHaF7OOLAx0HeY4q5itUD+SsmJdPIh2yKZkzAiA9MEIGCisGAQQBgjcCAQwxNDAy
# oBSAEgBNAGkAYwByAG8AcwBvAGYAdKEagBhodHRwOi8vd3d3Lm1pY3Jvc29mdC5j
# b20wDQYJKoZIhvcNAQEBBQAEggEAqmJA0xQg7ANSgcjbIFDOcI7HSWqbDNcmGL8Q
# 7v+cJyozRxFBzh9wD8IvBjfjPq35uWGx6R54HSvNhneR/OFnpebGWLO6WYaxb3cS
# IwbOsWr/ZDPp/dVzA4aPaHDYnRH27LFV4zP6iHUe5VF0OctmnVxXnSBSToPdBWXL
# GHCurj7AA6Z+gnWZLHnQ68YHc8TuiA2WWfkX37Guth7/xuypE2UQsgWpKksd46UU
# VHO4DWDvBvNBWYVOvnNBETFt2AUHJNVm8IHv9ZaByGDztX07dveb+3PMmavkjeoX
# HEvUaxGzY1GGTTvEQ6UWZM7TpWIkgtqIs5PewR2iPbfV1KSMpaGCFwAwghb8Bgor
# BgEEAYI3AwMBMYIW7DCCFugGCSqGSIb3DQEHAqCCFtkwghbVAgEDMQ8wDQYJYIZI
# AWUDBAIBBQAwggFRBgsqhkiG9w0BCRABBKCCAUAEggE8MIIBOAIBAQYKKwYBBAGE
# WQoDATAxMA0GCWCGSAFlAwQCAQUABCB7hUuHznNwCYT8cxDjembFOIZI5urveURl
# iec1qDDfjgIGY+55ncYnGBMyMDIzMDMxNTE4MzAxMy41OTVaMASAAgH0oIHQpIHN
# MIHKMQswCQYDVQQGEwJVUzETMBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMH
# UmVkbW9uZDEeMBwGA1UEChMVTWljcm9zb2Z0IENvcnBvcmF0aW9uMSUwIwYDVQQL
# ExxNaWNyb3NvZnQgQW1lcmljYSBPcGVyYXRpb25zMSYwJAYDVQQLEx1UaGFsZXMg
# VFNTIEVTTjo0OUJDLUUzN0EtMjMzQzElMCMGA1UEAxMcTWljcm9zb2Z0IFRpbWUt
# U3RhbXAgU2VydmljZaCCEVcwggcMMIIE9KADAgECAhMzAAABwFWkjcNkFcVLAAEA
# AAHAMA0GCSqGSIb3DQEBCwUAMHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNo
# aW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29y
# cG9yYXRpb24xJjAkBgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEw
# MB4XDTIyMTEwNDE5MDEyNVoXDTI0MDIwMjE5MDEyNVowgcoxCzAJBgNVBAYTAlVT
# MRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQK
# ExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJTAjBgNVBAsTHE1pY3Jvc29mdCBBbWVy
# aWNhIE9wZXJhdGlvbnMxJjAkBgNVBAsTHVRoYWxlcyBUU1MgRVNOOjQ5QkMtRTM3
# QS0yMzNDMSUwIwYDVQQDExxNaWNyb3NvZnQgVGltZS1TdGFtcCBTZXJ2aWNlMIIC
# IjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAvO1g+2NhhmBQvlGlCTOMaFw3
# jbIhUdDTqkaQhRpdHVb+huU/0HNhLmoRYvrp7z5vIoL1MPAkVBFWJIkrcG7sSred
# nyZwreY207C9n8XivL9ZBOQeiUeL/TMlJ6VinrcafbhdnkNO5JDlPozC9dGySiub
# ryds5GKtu69D1wNat9DIQl6alFO6pncZK4RIzfv+KzkM7RkY3vHphV0C8EFUpF+l
# ysaGJXFf9QsUUHwj9XKWHfc9BfhLoCReXUzvgrspdFmVnA9ATYXmidSjrshf8A+E
# 0/FpTdhXPI9XXqsZDHBqr7DlYoSCU3lvrVDRu1p5pHHf7s3kM16HpK6arDtY3ai1
# soASmEpv3C2N/y5MDBApDd4SpSkLMa7+6es/daeS7zdH1qdCa2RoJPM6Eh/6YmBf
# ofhfLQofKPJl34ALlZWK5AzVtFRNOXacoj6MAG2dT8Rc5fpKCH1E3n7Zje0dK24Q
# VfSv/YOxw52ECaMLlW5PhHT3ZINNaCmRgcHCTClOKzC2FOr03YBc2zPOW6bIVdXl
# oPmBMVaE+thXqPmANBw0YsncaOkVggjDb5O5VqOp98MklHpJoJI6pk5zAlx8/OtC
# 7FutrdtYNUC6ykXzMAPFuYkWGgx/W7A0itKW8WzYzwO3bAhprwznouGZmRiw2k8p
# en80BzqzdyPvbzTxQsMCAwEAAaOCATYwggEyMB0GA1UdDgQWBBQARMZ480jwpK3P
# 6quVWUEJ0c30hTAfBgNVHSMEGDAWgBSfpxVdAF5iXYP05dJlpxtTNRnpcjBfBgNV
# HR8EWDBWMFSgUqBQhk5odHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpb3BzL2Ny
# bC9NaWNyb3NvZnQlMjBUaW1lLVN0YW1wJTIwUENBJTIwMjAxMCgxKS5jcmwwbAYI
# KwYBBQUHAQEEYDBeMFwGCCsGAQUFBzAChlBodHRwOi8vd3d3Lm1pY3Jvc29mdC5j
# b20vcGtpb3BzL2NlcnRzL01pY3Jvc29mdCUyMFRpbWUtU3RhbXAlMjBQQ0ElMjAy
# MDEwKDEpLmNydDAMBgNVHRMBAf8EAjAAMBMGA1UdJQQMMAoGCCsGAQUFBwMIMA0G
# CSqGSIb3DQEBCwUAA4ICAQCtTh0EQn16kKQyCeVk9Vc10m6L0EwLRo3ATRouP7Yd
# 2hWeEB2Y4ZF4CJKe9qfXWGJKzV7tMUm6DAsBKYH/nT+8ybI8uJiHGnfnVi6Sh7gF
# jnTpfh1j1T90H/uLeoFjpOn/+eoCoJmorW5Gb2ezlTlo5I0kNAubxtCxqbLizuPN
# Pob8kRAKQgv+4/CC1JmiUFG0uKINlKj9SsHcrWeBBQHX62nNgziIwT44JqHrA02I
# 6cmQAi9BZcsf57OOLpRYlzoPH3x/+ldSySXAmyLq2uSbWtQuD84I/0ZgS/B5L3ew
# qTdiE1KbKX89MW5JqCK/yI/mAIQammAlHPqU9eZZTMPOHQs0XrpCijlk+qyo2JaH
# iySww6nuPqXzU3sEj3VW00YiVSayKEu1IrRzzX3La8qe6OqLTvK/6gu5XdKq7TT8
# 52nB6IP0QM+Budtr4Fbx4/svpKHGpK9/zBuaHHDXX5AoSksh/kSDYKfefQIhIfQJ
# JzoE3X+MimMJrgrwZXltb6j1IL0HY3qCpa03Ghgi0ITzqfkw3Man3G8kB1Ql+SeN
# ciPUj73Kn2veJenGLtT8JkUM9RUi0woO0iuY4tJnYuS+SeqavXUOWqUYVY19FIr1
# PLqpmWkbrO5xKjkyOHoAmLxjNbKjOnkAwft+1G00kulKqzqPbm+Sn+47JsGQFhNG
# bTCCB3EwggVZoAMCAQICEzMAAAAVxedrngKbSZkAAAAAABUwDQYJKoZIhvcNAQEL
# BQAwgYgxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYDVQQH
# EwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xMjAwBgNV
# BAMTKU1pY3Jvc29mdCBSb290IENlcnRpZmljYXRlIEF1dGhvcml0eSAyMDEwMB4X
# DTIxMDkzMDE4MjIyNVoXDTMwMDkzMDE4MzIyNVowfDELMAkGA1UEBhMCVVMxEzAR
# BgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1p
# Y3Jvc29mdCBDb3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3Rh
# bXAgUENBIDIwMTAwggIiMA0GCSqGSIb3DQEBAQUAA4ICDwAwggIKAoICAQDk4aZM
# 57RyIQt5osvXJHm9DtWC0/3unAcH0qlsTnXIyjVX9gF/bErg4r25PhdgM/9cT8dm
# 95VTcVrifkpa/rg2Z4VGIwy1jRPPdzLAEBjoYH1qUoNEt6aORmsHFPPFdvWGUNzB
# RMhxXFExN6AKOG6N7dcP2CZTfDlhAnrEqv1yaa8dq6z2Nr41JmTamDu6GnszrYBb
# fowQHJ1S/rboYiXcag/PXfT+jlPP1uyFVk3v3byNpOORj7I5LFGc6XBpDco2LXCO
# Mcg1KL3jtIckw+DJj361VI/c+gVVmG1oO5pGve2krnopN6zL64NF50ZuyjLVwIYw
# XE8s4mKyzbnijYjklqwBSru+cakXW2dg3viSkR4dPf0gz3N9QZpGdc3EXzTdEonW
# /aUgfX782Z5F37ZyL9t9X4C626p+Nuw2TPYrbqgSUei/BQOj0XOmTTd0lBw0gg/w
# EPK3Rxjtp+iZfD9M269ewvPV2HM9Q07BMzlMjgK8QmguEOqEUUbi0b1qGFphAXPK
# Z6Je1yh2AuIzGHLXpyDwwvoSCtdjbwzJNmSLW6CmgyFdXzB0kZSU2LlQ+QuJYfM2
# BjUYhEfb3BvR/bLUHMVr9lxSUV0S2yW6r1AFemzFER1y7435UsSFF5PAPBXbGjfH
# CBUYP3irRbb1Hode2o+eFnJpxq57t7c+auIurQIDAQABo4IB3TCCAdkwEgYJKwYB
# BAGCNxUBBAUCAwEAATAjBgkrBgEEAYI3FQIEFgQUKqdS/mTEmr6CkTxGNSnPEP8v
# BO4wHQYDVR0OBBYEFJ+nFV0AXmJdg/Tl0mWnG1M1GelyMFwGA1UdIARVMFMwUQYM
# KwYBBAGCN0yDfQEBMEEwPwYIKwYBBQUHAgEWM2h0dHA6Ly93d3cubWljcm9zb2Z0
# LmNvbS9wa2lvcHMvRG9jcy9SZXBvc2l0b3J5Lmh0bTATBgNVHSUEDDAKBggrBgEF
# BQcDCDAZBgkrBgEEAYI3FAIEDB4KAFMAdQBiAEMAQTALBgNVHQ8EBAMCAYYwDwYD
# VR0TAQH/BAUwAwEB/zAfBgNVHSMEGDAWgBTV9lbLj+iiXGJo0T2UkFvXzpoYxDBW
# BgNVHR8ETzBNMEugSaBHhkVodHRwOi8vY3JsLm1pY3Jvc29mdC5jb20vcGtpL2Ny
# bC9wcm9kdWN0cy9NaWNSb29DZXJBdXRfMjAxMC0wNi0yMy5jcmwwWgYIKwYBBQUH
# AQEETjBMMEoGCCsGAQUFBzAChj5odHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtp
# L2NlcnRzL01pY1Jvb0NlckF1dF8yMDEwLTA2LTIzLmNydDANBgkqhkiG9w0BAQsF
# AAOCAgEAnVV9/Cqt4SwfZwExJFvhnnJL/Klv6lwUtj5OR2R4sQaTlz0xM7U518Jx
# Nj/aZGx80HU5bbsPMeTCj/ts0aGUGCLu6WZnOlNN3Zi6th542DYunKmCVgADsAW+
# iehp4LoJ7nvfam++Kctu2D9IdQHZGN5tggz1bSNU5HhTdSRXud2f8449xvNo32X2
# pFaq95W2KFUn0CS9QKC/GbYSEhFdPSfgQJY4rPf5KYnDvBewVIVCs/wMnosZiefw
# C2qBwoEZQhlSdYo2wh3DYXMuLGt7bj8sCXgU6ZGyqVvfSaN0DLzskYDSPeZKPmY7
# T7uG+jIa2Zb0j/aRAfbOxnT99kxybxCrdTDFNLB62FD+CljdQDzHVG2dY3RILLFO
# Ry3BFARxv2T5JL5zbcqOCb2zAVdJVGTZc9d/HltEAY5aGZFrDZ+kKNxnGSgkujhL
# mm77IVRrakURR6nxt67I6IleT53S0Ex2tVdUCbFpAUR+fKFhbHP+CrvsQWY9af3L
# wUFJfn6Tvsv4O+S3Fb+0zj6lMVGEvL8CwYKiexcdFYmNcP7ntdAoGokLjzbaukz5
# m/8K6TT4JDVnK+ANuOaMmdbhIurwJ0I9JZTmdHRbatGePu1+oDEzfbzL6Xu/OHBE
# 0ZDxyKs6ijoIYn/ZcGNTTY3ugm2lBRDBcQZqELQdVTNYs6FwZvKhggLOMIICNwIB
# ATCB+KGB0KSBzTCByjELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24x
# EDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlv
# bjElMCMGA1UECxMcTWljcm9zb2Z0IEFtZXJpY2EgT3BlcmF0aW9uczEmMCQGA1UE
# CxMdVGhhbGVzIFRTUyBFU046NDlCQy1FMzdBLTIzM0MxJTAjBgNVBAMTHE1pY3Jv
# c29mdCBUaW1lLVN0YW1wIFNlcnZpY2WiIwoBATAHBgUrDgMCGgMVABAQ7ExF19Kk
# wVL1E3Ad8k0Peb6doIGDMIGApH4wfDELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldh
# c2hpbmd0b24xEDAOBgNVBAcTB1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBD
# b3Jwb3JhdGlvbjEmMCQGA1UEAxMdTWljcm9zb2Z0IFRpbWUtU3RhbXAgUENBIDIw
# MTAwDQYJKoZIhvcNAQEFBQACBQDnu+VmMCIYDzIwMjMwMzE1MTQzNDE0WhgPMjAy
# MzAzMTYxNDM0MTRaMHcwPQYKKwYBBAGEWQoEATEvMC0wCgIFAOe75WYCAQAwCgIB
# AAICDVQCAf8wBwIBAAICEzcwCgIFAOe9NuYCAQAwNgYKKwYBBAGEWQoEAjEoMCYw
# DAYKKwYBBAGEWQoDAqAKMAgCAQACAwehIKEKMAgCAQACAwGGoDANBgkqhkiG9w0B
# AQUFAAOBgQCYIvc2116pSIiWdxBnfJlraXp6yVgrmKe7UffFNK74m7qt0HSCj09i
# sU9mNzNbddyS1T3Q9sw5LYAi1uVYFq8RBmPaA1X9zs8XTUBul2BI39At2yLpbPSa
# fsOs1JkFSk41OPfaTClHF0NaGRN4a9+sN+Ri/TyeEh7ZL8FbJYPOszGCBA0wggQJ
# AgEBMIGTMHwxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5ndG9uMRAwDgYD
# VQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9yYXRpb24xJjAk
# BgNVBAMTHU1pY3Jvc29mdCBUaW1lLVN0YW1wIFBDQSAyMDEwAhMzAAABwFWkjcNk
# FcVLAAEAAAHAMA0GCWCGSAFlAwQCAQUAoIIBSjAaBgkqhkiG9w0BCQMxDQYLKoZI
# hvcNAQkQAQQwLwYJKoZIhvcNAQkEMSIEIFJuFWV93KGKGM7SovXJwLSuAp7mcF8z
# /Qy+O9PKvIqUMIH6BgsqhkiG9w0BCRACLzGB6jCB5zCB5DCBvQQgWvFYolIIXME0
# zK/W6XsCkkYX7lYNb9yA8JxwY04Pk08wgZgwgYCkfjB8MQswCQYDVQQGEwJVUzET
# MBEGA1UECBMKV2FzaGluZ3RvbjEQMA4GA1UEBxMHUmVkbW9uZDEeMBwGA1UEChMV
# TWljcm9zb2Z0IENvcnBvcmF0aW9uMSYwJAYDVQQDEx1NaWNyb3NvZnQgVGltZS1T
# dGFtcCBQQ0EgMjAxMAITMwAAAcBVpI3DZBXFSwABAAABwDAiBCAm5eU/p1mirEIE
# 4/tvetskcN910UbD0iYCwnat0JUlnjANBgkqhkiG9w0BAQsFAASCAgB7UyPQxXhN
# KawBcjxtzgj9B4POlovGRgF5xwxWhNSRxCGje+AgMbbI6m0srZXvhXGysFm7MyQu
# QtEl/dd0NZDxVwd50Pxn2wVjs36NX8q/8k8l/jJqqbnsfPBq2X1n8v/J+i/fxSLx
# 0pUipMO5cUEjZABc3SsCwQav5QOFZ5300bMfpHPqSSk20eByyx3z7p4QPIIsxjm9
# BslIGTqui1JpiwVNAe9roDwQ+lf2M60dc8334nigo/CNVDR0gIRYTBBUeoCDgCT4
# I0y2oRFBcpXm00Nx3EVYWE+Bzco2urBDpFWicy1YkfLRCNlJ2C7so3wojYd7lgy+
# 5IuMAGi/LTbeaY3PEsqxkei3qDrjnj8OUejDXnBKQozv9iNAFGeoqjEBzEtY+I/p
# 6ABT2SeluyVNBjlzvTLKD5agiUy1W4xkO9EHFqbTFbE9BYlCgvPB/UkT159K4QPT
# V9qj+7OG8cy/g1QmKV/hSBmXz8ovQekywyQYW+RXqA+5hpM2FAW0ZWTxBQ/qVdch
# TBIPFQ4T652xDY2L8laYmrpUvGkDIkCcaLdi/8oNZvsZR+u/jcNq6Mc36U1W06Zb
# E39FGjczyZhlD6mZ3mQw9nYBcdEiuNgzBPq4RKBOBMoEgmtRdqZOY5BPOVfjH8ez
# pj/q+A1seBo7pyKVvHXErEgJmUlRU7C/wA==
# SIG # End signature block
