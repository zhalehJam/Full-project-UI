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
using Microsoft.AspNetCore.WebUtilities;
using Ticketing.Models.Persons.Dto;

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
            var request = new HttpRequestMessage(HttpMethod.Get, "Program/GetAllPrograms");
            List<ProgramDto> programDtos = new List<ProgramDto>();
            var e= await _httpClient.SendAsync(request);
            var response = await _httpClient.GetAsync("Program/GetAllPrograms");
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

        public async Task<ProgramDto> GetProgramById(Guid programId)
        {
            ProgramDto? programDtos = new ProgramDto();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", programId.ToString());
            string request = QueryHelpers.AddQueryString("Person/GetAllPersonsByPage", parameters);
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            programDtos = GetProgramDtoFromContent(content).FirstOrDefault();

            return programDtos;
        }
    }
}
