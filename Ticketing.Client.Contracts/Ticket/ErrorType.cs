using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ticketing.Client.Contracts.Ticket
{
    public enum ErrorType
    {
        [Display(Name = "خطای سیستمی")]         
        SystemError = 1,

        [Display(Name = "خطای کاربر")]
        UserError = 2
    }
}
