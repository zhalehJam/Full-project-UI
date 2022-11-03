using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Cilient.Contracts.Ticket;

namespace Ticketing.Models.Tickets.Dto
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        public int PersonID { get; set; }
        public string PersonName { get; set; }
        public Guid PersonPartId { get; set; }
        public Guid PersonCenterId { get; set; }
        public string PersonPartName { get; set; }
        public string PersonCenterName { get; set; }
        public Guid ProgramId { get; set; }
        public string ProgramName { get; set; }
        public int ErrorType { get; set; }
        public string ErrorTypeName { get; set; }
        public int Typeid { get; set; }
        public string TicketTypeName { get; set; }
        public string? ErrorDescription { get; set; }
        public string? SolutionDescription { get; set; }
        public DateTime TicketTime { get; set; }
        public int TicketConditionid { get; set; }
        public string TicketConditionTypeName { get; set; }
        public int SupporterPersonID { get; set; }
        public string SupporterPersonName { get; set; }
    }
}
