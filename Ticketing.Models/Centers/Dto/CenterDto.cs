using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Models.Centers.Dto
{
    public class CenterDto
    {
        public Guid Id { get; set; }
        public string CenterName { get; set; } = "";
        public int CenterID { get; set; }
        public List<PartDto> parts { get; set; }
    }
}
