namespace Ticketing.Models.Programs.Command
{
    public class UpdateProgramLinkCommand    
    {
        public Guid Id { get; set; }
        public string ProgramLink { get; set; }
    }
}
