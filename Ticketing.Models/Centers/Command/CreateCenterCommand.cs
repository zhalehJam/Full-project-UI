namespace Ticketing.Models.Centers.Command
{
    public class CreateCenterCommand 
    {
        public string? CenterName { get; set; }
        public int CenterID { get; set; }
    }
}
