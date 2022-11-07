using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Centers.Dto;
using Ticketing.Models.Persons.Dto;
using Ticketing.Models.Persons.Repository;
using Microsoft.AspNetCore.WebUtilities;

namespace Ticketing.Repository.Persons
{
    public class PersonRepository : IPersonRepository
    {
        private readonly HttpClient _httpClient;

        public PersonRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<PersonDto>> GetAllPersons()
        {
            List<PersonDto> persons = new List<PersonDto>();
            var response = await _httpClient.GetAsync("Person/GetAllPersons");
            var content = await response.Content.ReadAsStringAsync();
            persons = GetPersonDtoFromContent(content);
            return persons;
        }

        public async Task<PersonDto> GetPersonById(Guid Id)
        {
            PersonDto? personDtos = new PersonDto();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", Id.ToString()); 
            string request = QueryHelpers.AddQueryString("Person/GetPersonById", parameters);
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            personDtos = GetPersonDtoFromContent(content).FirstOrDefault();

            return personDtos;
        }

        private static List<PersonDto> GetPersonDtoFromContent(string content)
        {
            List<PersonDto> personDtos = new List<PersonDto>();
            JArray jsonResponse = JArray.Parse(content);

            foreach(var item in jsonResponse)
            {
                PersonDto? dto = JsonConvert.DeserializeObject<PersonDto>(item.ToString());
                personDtos.Add(dto);
            }
            return personDtos;
        }

        public async Task<List<PersonDto>> GetAllPersons(string page, string pageSize)
        {
            List<PersonDto> personDtos = new List<PersonDto>();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("PageNumber", page);
            parameters.Add("PageSize", pageSize);
            string request = QueryHelpers.AddQueryString("Person/GetAllPersonsByPage", parameters);
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            personDtos = GetPersonDtoFromContent(content);

            return personDtos;
        }
    }
}
