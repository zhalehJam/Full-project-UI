using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Models.Persons.Dto
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string? PersonName { get; set; } = "";
        public int PersonID { get; set; }
        public Guid PartId { get; set; }
        public string? PartName { get; set; } = "";
        public string? CenterName { get; set; } = "";
    }
}
