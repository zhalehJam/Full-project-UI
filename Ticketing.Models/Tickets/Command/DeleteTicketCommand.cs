namespace Ticketing.Models.Tickets.Command
{
    public class DeleteTicketCommand 
    {
        public Guid Id { get; set; }
        public int SupporterUser { get; set; }
    }
}
