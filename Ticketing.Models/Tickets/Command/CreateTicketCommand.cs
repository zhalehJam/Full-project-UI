using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Client.Contracts.Ticket;

namespace Ticketing.Models.Tickets.Command
{
    public class CreateTicketCommand
    {
        public int SupporterPersonID { get; set; }
        public int PersonID { get; set; }
        public Guid ProgramId { get; set; }
        public ErrorType ErrorType { get; set; }
        public TicketType Type { get; set; }
        public string ErrorDiscription { get; set; }
        public string SolutionDiscription { get; set; }
        public DateTime TicketTime { get; set; }
        public TicketCondition TicketCondition { get; set; }
    }
}
