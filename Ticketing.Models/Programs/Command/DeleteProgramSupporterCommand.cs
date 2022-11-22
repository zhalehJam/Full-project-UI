namespace Ticketing.Models.Programs.Command
{
    public class DeleteProgramSupporterCommand    
    {
        public Guid ProgramId { get; set; }
        public Int32 SupporterID { get; set; }
    }
}
