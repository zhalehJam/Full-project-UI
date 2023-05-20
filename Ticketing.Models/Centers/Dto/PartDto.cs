namespace Ticketing.Models.Centers.Dto
{
    public class PartDto
    {
        public Guid Id { get; set; }
        public Guid Center { get; set; }
        public string? PartName { get; set; }
        public int PartID { get; set; }
    }
}
