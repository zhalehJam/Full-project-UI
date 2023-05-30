using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Programs.Command;
using Ticketing.Models.Programs.Dto;

namespace Ticketing.Models.Programs.Repository
{
    public interface IProgramRepository
    {
        Task<List<ProgramDto>> GetAllProgram();
        Task<ProgramDto> GetProgramById(Guid programId);
        Task<List<ProgramDto>> GetSupporterProgramsList(int supporterCode);
        Task CreateProgram(CreateProgramCommand createProgramCommand);
        Task UpdateProgramLink(UpdateProgramLinkCommand updateProgramLinkComand);
        Task AddProgramSupporter(AddProgramSupporterCommand addProgramSupporter);
        Task DeletePorogramSupporter(DeleteProgramSupporterCommand deleteProgramSupporter);
        Task DeleteProgram(DeleteProgramCommand deleteProgramCommand);
    }
}
