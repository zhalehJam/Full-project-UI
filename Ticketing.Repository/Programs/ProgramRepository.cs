using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Centers.Dto;
using Ticketing.Models.Programs.Dto;
using Ticketing.Models.Programs.Repository;

namespace Ticketing.Repository.Programs
{
    public class ProgramRepository : IProgramRepository
    {
        private readonly HttpClient _httpClient;

        public ProgramRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProgramDto>> GetAllProgram()
        {
            List<ProgramDto> programDtos = new List<ProgramDto>();
            var response = await _httpClient.GetAsync("https://localhost:44359/api/Program/GetAllPrograms");
            var content = await response.Content.ReadAsStringAsync();
            programDtos = GetProgramDtoFromContent(content);
            return programDtos;

        }
        private static List<ProgramDto> GetProgramDtoFromContent(string content)
        {
            List<ProgramDto> ticketDtos = new List<ProgramDto>();
            JArray jsonResponse = JArray.Parse(content);

            foreach(var item in jsonResponse)
            {
                ProgramDto? dto = JsonConvert.DeserializeObject<ProgramDto>(item.ToString());
                ticketDtos.Add(dto);
            }
            return ticketDtos;
        }

        public Task<ProgramDto> GetProgramById(Guid programId)
        {
            throw new NotImplementedException();
        }
    }
}
