using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Client.Contracts.Ticket;

namespace Ticketing.Models.Tickets.Dto
{
    public class TicketDto
    {
        public Guid Id { get; set; }
         
        [Range(1,int.MaxValue, ErrorMessage = "کد پرسنل الزامی است")]
        public int PersonID { get; set; }

        public string PersonName { get; set; }

        public Guid PersonPartId { get; set; }
         
        public Guid PersonCenterId { get; set; }
         
        public string PersonPartName { get; set; }

        public string PersonCenterName { get; set; }

        public Guid ProgramId { get; set; } 

        [Required(ErrorMessage = "نام برنامه الزامی است")]
        public string ProgramName { get; set; }

        public int ErrorTypeid { get; set; }

        [Required(ErrorMessage = "اطلاعات خطا الزامی است")] 
        public string ErrorTypeName { get; set; }

        public int Typeid { get; set; }

        [Required(ErrorMessage = "نوع تیکت الزامی است")]
        public string TicketTypeName { get; set; }

        [Required(ErrorMessage = "شرح خطا الزامی است")] 
        public string? ErrorDiscription { get; set; } = "";
         
        public string? SolutionDescription { get; set; } = "";

        public DateTime TicketTime { get; set; } = DateTime.Now;

        public int TicketConditionid { get; set; }

        [Required(ErrorMessage = "وضعیت تیکت الزامی است")]
        public string TicketConditionTypeName { get; set; }

        public int SupporterPersonID { get; set; }

        public string SupporterPersonName { get; set; }
    }
}
