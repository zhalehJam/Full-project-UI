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
            var response = await _httpClient.GetAsync("https://localhost:44359/api/Person/GetAllPersons");
            var content = await response.Content.ReadAsStringAsync();
            persons = GetPersonDtoFromContent(content);
            return persons;
        }

        public Task<PersonDto> GetPersonById(Guid Id)
        {
            throw new NotImplementedException();
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
    }
}
