using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
using Ticketing.Models.Persons.Command;
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
            personDtos = GetPersonDtoFromContent(content).First();

            return personDtos;
        }
        public async Task<PersonDto> GetPersonInfoByPersonelCode(int PersonnelCode)
        {
            PersonDto personDtos = new PersonDto();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("personnelCode", PersonnelCode.ToString());
            string request = QueryHelpers.AddQueryString("Person/GetPersonInfoByPersonelCode", parameters);
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            personDtos = JsonConvert.DeserializeObject<PersonDto>(content);

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

        public async Task CreatePerson(CreatePersonCommand createPersonCommand)
        {
            await SendRequest<CreatePersonCommand>(createPersonCommand, HttpMethod.Post, "Person/CreatePerson");
        }

        public async Task UpdatePerson(UpdatePersonCommand updatePersonCommand)
        {
            await SendRequest<UpdatePersonCommand>(updatePersonCommand, HttpMethod.Put, "Person/UpdatePerson");
        }

        public async Task DeletePerson(DeletePersonCommand deletePersonCommand)
        {
            await SendRequest<DeletePersonCommand>(deletePersonCommand, HttpMethod.Delete, "Person/DeletePerson");
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

        private List<PersonDto> GetPersonDtoFromContent(string content)
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

        public async Task<string> GetUserPhoto(int personnelCode)
        { 
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("personnelCode", personnelCode.ToString());
            string request = QueryHelpers.AddQueryString("Person/GetUserPhoto", parameters);
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
              return JsonConvert.DeserializeObject<string>(content);
             
        }
    }
}
