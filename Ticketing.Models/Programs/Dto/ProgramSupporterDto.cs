namespace Ticketing.Models.Programs.Dto
{
    public class ProgramSupporterDto
    {
        public Guid Id { get; set; }
        public Guid ProgramId { get; set; }
        public string ProgramName { get; set; } = "";
        public int SupporterpersonID { get; set; }
        public string SupporterName { get; set; } = "";
    }

}

