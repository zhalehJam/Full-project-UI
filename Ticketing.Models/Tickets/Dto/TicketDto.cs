using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Cilient.Contracts.Ticket;

namespace Ticketing.Models.Tickets.Dto
{
    public class TicketDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "program required")]
        public int PersonID { get; set; }

        public string PersonName { get; set; }
        public Guid PersonPartId { get; set; }
        public Guid PersonCenterId { get; set; }
        public string PersonPartName { get; set; }
        public string PersonCenterName { get; set; }
        [Required(ErrorMessage = "program required")]
        public Guid ProgramId { get; set; }
        [Required(ErrorMessage = "program required")]
        public string ProgramName { get; set; }
        public int ErrorTypeid { get; set; }
        public string ErrorTypeName { get; set; }
        public int Typeid { get; set; }
        public string TicketTypeName { get; set; }
        public string? ErrorDescription { get; set; }
        public string? SolutionDescription { get; set; }
        public DateTime TicketTime { get; set; } = DateTime.Now;
        public int TicketConditionid { get; set; }
        public string TicketConditionTypeName { get; set; }
        public int SupporterPersonID { get; set; }
        public string SupporterPersonName { get; set; }
    }
}
