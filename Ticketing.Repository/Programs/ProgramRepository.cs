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
using Ticketing.Models.Programs.Command;
using Ticketing.Models.Centers.Command;
using System.Net;
using Ticketing.Models.Persons.Command;
using System.Reflection.Metadata;

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
            var e = await _httpClient.SendAsync(request);
            var response = await _httpClient.GetAsync("Program/GetAllPrograms");
            var content = await response.Content.ReadAsStringAsync();
            programDtos = GetProgramDtoFromContent(content);
            return programDtos;

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

        public async Task<List<ProgramDto>> GetSupporterProgramsList(int supporterCode)
        { 
            List<ProgramDto> programDtos = new List<ProgramDto>();
            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "supporterCode", supporterCode.ToString() }
            };
            string request = QueryHelpers.AddQueryString("Program/GetSupporterProgramsList", parameters); 
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            programDtos = GetProgramDtoFromContent(content);
            return programDtos;
        }

        public async Task CreateProgram(CreateProgramCommand createProgramCommand)
        {
            await SendRequest<CreateProgramCommand>(createProgramCommand, HttpMethod.Post, "Program");
        }

        public async Task UpdateProgramLink(UpdateProgramLinkCommand updateProgramLinkComand)
        {
            await SendRequest<UpdateProgramLinkCommand>(updateProgramLinkComand, HttpMethod.Put, "Program/UpdateProgramLink");
        }

        public async Task AddProgramSupporter(AddProgramSupporterCommand addProgramSupporter)
        {
            await SendRequest<AddProgramSupporterCommand>(addProgramSupporter, HttpMethod.Put, "Program/AddPrgramSupporter");
        }

        public async Task DeletePorogramSupporter(DeleteProgramSupporterCommand deleteProgramSupporter)
        {
            await SendRequest<DeleteProgramSupporterCommand>(deleteProgramSupporter, HttpMethod.Put, "Program/DeleteProgramSupporter");
        }

        public async Task DeleteProgram(DeleteProgramCommand deleteProgramCommand)
        {
            await SendRequest<DeleteProgramCommand>(deleteProgramCommand, HttpMethod.Delete, "Program");
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

        private async Task SendRequest<T>(T command, HttpMethod httpMethod, string uri)
        {
            var postRequest = new HttpRequestMessage(httpMethod, uri)
            {
                Content = JsonContent.Create(command)
            };
            var postResponse = await _httpClient.SendAsync(postRequest);
            if(!postResponse.IsSuccessStatusCode)
            {
                var error = await postResponse.Content.ReadAsStringAsync();
                string errormessage = error.Split("\r")[0].Split(":")[1];
                if(postResponse.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new Exception(errormessage);
                }
            }
        }
    }
}
