using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Models.Centers.Command
{
    public class AddPartCommand 
    {
        public Guid CenterId { get; set; }
        public string? PartName { get; set; }
        public int PartID { get; set; }
    }
}
