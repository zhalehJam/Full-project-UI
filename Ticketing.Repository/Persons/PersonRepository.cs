using Framework.Pagination;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Ticketing.Client.Contracts.Persons;
using Ticketing.Client.Contracts.Ticket.Queries;
using Ticketing.Models.Persons.Command;
using Ticketing.Models.Persons.Dto;
using Ticketing.Models.Persons.Repository;

namespace Ticketing.Repository.Persons
{
    public class PersonRepository : IPersonRepository
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        private readonly JsonSerializerOptions _options;
        public PersonRepository(IHttpClientFactory clientFactory, TokenProvider tokenProvider)
        {
            _httpClient = clientFactory.CreateClient("API");
            _tokenProvider = tokenProvider;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenProvider.AccessToken}");
            _httpClient.DefaultRequestHeaders.Add("X-Pagination", "CustomValue");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

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
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                personDtos = GetPersonDtoFromContent(content).First();
            }
            return personDtos;
        }
        public async Task<PersonDto> GetPersonInfoByPersonelCode(int PersonnelCode)
        {
            PersonDto personDtos = new PersonDto();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("personnelCode", PersonnelCode.ToString());
            string request = QueryHelpers.AddQueryString("Person/GetPersonInfoByPersonelCode", parameters);
            var response = await _httpClient.GetAsync(request);
            if(response.StatusCode==HttpStatusCode.Unauthorized)
            { 
                return personDtos;
            }
            
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

        public async Task<PagingResponse<PersonDto>> GetPersonInfoByFilters(GetPersonInfoByFiltersQuery getPersonInfoByFiltersQuery)
        {
            //GetPersonInfoBySelectedInfo
            List<PersonDto> personDtos = new List<PersonDto>(); 
            var response = await _httpClient.GetAsync($"Person/GetPersonInfoByFilters?{getPersonInfoByFiltersQuery.ToQuery()}");
            var content = await response.Content.ReadAsStringAsync();
            personDtos = GetPersonDtoFromContent(content);
            var pagingResponse = new PagingResponse<PersonDto>
            {
                Items = GetPersonDtoFromContent(content),
                MetaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), _options)
            };
            return pagingResponse;
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
            if (!postResponse.IsSuccessStatusCode)
            {
                var error = await postResponse.Content.ReadAsStringAsync();
                string errormessage = error.Split("\r")[0].Split(":")[1];
                if (postResponse.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new Exception(errormessage);
                }
            }
        }

        private List<PersonDto> GetPersonDtoFromContent(string content)
        {
            List<PersonDto> personDtos = new List<PersonDto>();
            JArray jsonResponse = JArray.Parse(content);

            foreach (var item in jsonResponse)
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
