using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Client.Contracts.Ticket
{
    public enum TicketType
    {
        [Display(Name ="پشتیبانی")]
        Supporting = 1,
        [Display(Name ="توسعه")]
        Developing = 2
    }
}
