using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Models.Centers.Dto
{
    public class CenterDto
    {
        public Guid Center { get; set; }
        public string CenterName { get; set; }
        public int CenterID { get; set; }
        public IList<PartDto> parts { get; set; }
    }
}
