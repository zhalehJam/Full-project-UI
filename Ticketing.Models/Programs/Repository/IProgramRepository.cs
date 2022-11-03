using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Programs.Dto;

namespace Ticketing.Models.Programs.Repository
{
    public interface IProgramRepository
    {
        Task<List<ProgramDto>> GetAllProgram();
        Task<ProgramDto> GetProgramById(Guid programId);
    }
}
