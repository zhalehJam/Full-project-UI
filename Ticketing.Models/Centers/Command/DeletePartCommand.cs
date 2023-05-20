namespace Ticketing.Models.Centers.Command
{
    public class DeletePartCommand
    {
        public Guid CenterId { get; set; }
        public int PartID { get; set; }
    }
}
