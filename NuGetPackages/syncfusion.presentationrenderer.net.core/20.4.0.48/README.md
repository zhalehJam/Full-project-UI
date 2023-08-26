### Syncfusion ASP.NET Core PowerPoint to PDF converter library

The Syncfusion [.NET Core PowerPoint (Presentation) library](https://www.syncfusion.com/powerpoint-framework/net-core/powerpoint-library?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) (Essential Presentation) converts a PowerPoint presentation to PDF with just five lines of code and also it does not require Adobe and Microsoft PowerPoint application to be installed in the machine. It preserves the original appearance of the PowerPoint presentation in the converted PDF document.

![ASP.NET Core PowerPoint Library](https://cdn.syncfusion.com/nuget-readme/fileformats/net-pptx-to-pdf.png)

[Features overview](https://www.syncfusion.com/powerpoint-framework/net-core/powerpoint-library?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) | [Docs](https://help.syncfusion.com/file-formats/presentation/overview?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) | [API Reference](https://help.syncfusion.com/cr/file-formats/Syncfusion.Presentation.html?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) | [Online Demo](https://ej2.syncfusion.com/aspnetcore/Presentation/Default?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget#/bootstrap5) | [GitHub Examples](https://github.com/syncfusion/ej2-aspnetcore-samples/tree/master/Controllers/Presentation?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) | [Blogs](https://www.syncfusion.com/blogs/?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget&s=powerpoint) | [Support](https://support.syncfusion.com/create?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) | [Forums](https://www.syncfusion.com/forums?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) | [Feedback](https://www.syncfusion.com/feedback/powerpoint?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)

### Key Features

* Convert entire [PowerPoint presentation to PDF](https://help.syncfusion.com/file-formats/presentation/presentation-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget).
* Convert specific [PowerPoint slide to PDF](https://help.syncfusion.com/file-formats/presentation/presentation-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget).
* Convert PowerPoint presentation to PDF with,
  * [Auto shapes](https://help.syncfusion.com/file-formats/presentation/working-with-shapes?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Images](https://help.syncfusion.com/file-formats/presentation/working-with-images?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Tables](https://help.syncfusion.com/file-formats/presentation/working-with-tables?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Charts](https://help.syncfusion.com/file-formats/presentation/working-with-charts?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget#chart-to-image-conversion)
  * [Connectors](https://help.syncfusion.com/file-formats/presentation/create-edit-connectors-in-powerpoint-slides-cs-vb-net?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Header and Footer](https://help.syncfusion.com/file-formats/presentation/working-with-headers-and-footers?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [SmartArts](https://help.syncfusion.com/file-formats/presentation/smartart?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [OLE Objects](https://help.syncfusion.com/file-formats/presentation/working-with-ole-objects?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Group shapes](https://help.syncfusion.com/file-formats/presentation/working-with-shapes?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Rich-text formatting](https://help.syncfusion.com/file-formats/presentation/working-with-paragraphs?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Paragraph alignments](https://help.syncfusion.com/file-formats/presentation/working-with-paragraphs?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Hyperlinks](https://help.syncfusion.com/file-formats/presentation/working-with-hyperlinks?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Handouts](https://help.syncfusion.com/file-formats/presentation/presentation-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [Notes Pages](https://help.syncfusion.com/file-formats/presentation/presentation-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
  * [PDF Conformance](https://help.syncfusion.com/file-formats/presentation/presentation-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)
* Support to run the Presentation applications in multi-thread and its thread safe.

### System Requirements
* [System Requirements](https://help.syncfusion.com/file-formats/installation-and-upgrade/system-requirements?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget).

### Getting Started

You can fetch the Syncfusion .NET Core PowerPoint library NuGet by simply running the command `Install-Package Syncfusion.PresentationRenderer.Net.Core` from the Package Manager Console in Visual Studio.

Try the following code example to convert the PowerPoint Presentation to PDF.

```csharp
//Namespaces to perform PPTX to PDF conversion
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Presentation;
using Syncfusion.PresentationToPdfConverter;
using Syncfusion.Pdf;

using (FileStream fileStreamInput = new FileStream("Template.pptx", FileMode.Open, FileAccess.Read))
{
	//Open the existing PowerPoint presentation.
	using (IPresentation pptxDoc = Presentation.Open(fileStreamInput))
	{
		//Convert the PowerPoint document to PDF document.
		using (PdfDocument pdfDocument = PresentationToPdfConverter.Convert(pptxDoc))
		{
			//Creates an instance of memory stream.
			using (MemoryStream pdfStream = new MemoryStream())
			{
				//Save the converted PDF document to memory stream.
				pdfDocument.Save(pdfStream);
			}
		}
	}
}
```

Try the following code example to convert the PowerPoint Presentation to Image.

```csharp
//Namespaces to perform PPTX to PDF conversion
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Presentation;
using Syncfusion.PresentationToPdfConverter;
using Syncfusion.Pdf;

using (FileStream fileStreamInput = new FileStream("Template.pptx", FileMode.Open, FileAccess.Read))
{
	//Open the existing PowerPoint presentation.
	using (IPresentation pptxDoc = Presentation.Open(fileStreamInput))
	{
		//Initialize PresentationRenderer to perform image conversion.
		pptxDoc.PresentationRenderer = new PresentationRenderer();
		//Converts entire Presentation to image stream.
		Stream[] streams = pptxDoc.RenderAsImages(ExportImageFormat.Jpeg);
		foreach (Stream stream in streams)
		{
			//Create the output image file stream.
			using (FileStream fileStreamOutput = File.Create("Output_" + Guid.NewGuid().ToString() + ".jpg"))
			{
				//Copy the converted image stream into created output stream.
				stream.CopyTo(fileStreamOutput);
			}
		}
	}
}
```

For more information to get started, refer to our [Getting Started Documentation page](https://help.syncfusion.com/file-formats/presentation/presentation-to-pdf?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget).

### License
This is a commercial product and requires a paid license for possession or use. Syncfusion’s licensed software, including this component, is subject to the terms and conditions of [Syncfusion's EULA](https://www.syncfusion.com/eula/es/?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget). You can purchase a license [here](https://www.syncfusion.com/sales/products?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) or start a free 30-day trial [here](https://www.syncfusion.com/account/manage-trials/start-trials?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget).

### About Syncfusion
Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion has more than 27,000+ customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.

Today, we provide 1700+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [ASP.NET Web Forms](https://www.syncfusion.com/jquery/aspnet-web-forms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), and [jQuery](https://www.syncfusion.com/jquery-ui-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)), mobile ([.NET MAUI (Preview)](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [WPF](https://www.syncfusion.com/wpf-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [.NET MAUI (Preview)](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget), and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget)). We provide ready-to-deploy enterprise software for dashboards, reports, data integration, and big data processing. Many customers have saved millions in licensing fees by deploying our software.
____

[sales@syncfusion.com](mailto:sales@syncfusion.com?Subject=Syncfusion%20ASPNET%20Core%20PresentationRenderer%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=aspnetcore-presentation-renderer-nuget) | Toll Free: 1-888-9 DOTNET
