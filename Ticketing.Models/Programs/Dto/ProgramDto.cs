using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Models.Programs.Dto
{
    public class ProgramDto
    {
        public Guid Id { get; set; }
        public string ProgamName { get; set; }
        public string ProgramLink { get; set; }
        public List<ProgramSupporterDto> Supporters { get; set; }
    }
}
