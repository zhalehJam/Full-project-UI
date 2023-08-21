using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ticketing.Client.Contracts.Persons
{
    public enum RoleType
    {
        [Display(Name = "ادمین")]
        Admin = 1,
        [Display(Name = "پشتیبان")]
        Supporter = 2,
        [Display(Name = "کاربر")]
        User = 3,
        [Display(Name = "P.O")]
        ProductOwner = 4,
        [Display(Name = "توسعه دهنده")]
        Developer = 5
    }
}
