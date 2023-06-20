using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Client.Contracts.Persons;

namespace Ticketing.Models.Persons.Command
{
    public class CreatePersonCommand
    {
        public string? Name { get; set; }
        public Int32 PersonID { get; set; }
        //public Guid CenterId { get; set; }
        public Guid PartId { get; set; }
        public RoleType PersonRoleType { get; set; }
    }
}
