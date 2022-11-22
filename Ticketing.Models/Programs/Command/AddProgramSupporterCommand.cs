using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Models.Programs.Command
{
    public class AddProgramSupporterCommand    
    {
        public Guid ProgramId { get; set; }
        public Int32 SupporterID { get; set; }
    }
}
