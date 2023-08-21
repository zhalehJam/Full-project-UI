using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ticketing.Client.Contracts.Ticket
{
    public enum TicketCondition
    {
        [Display(Name = "در حال انجام")]
        OnGoing = 1,
        [Display(Name = "خاتمه یافته")]
        Finish = 2
    }
}
