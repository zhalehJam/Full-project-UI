using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq; 
using System.Net;
using System.Net.Http.Json; 
using Ticketing.Models.Centers.Command;
using Ticketing.Models.Centers.Dto;
using Ticketing.Models.Centers.Repository;

namespace Ticketing.Repository.Centers
{
    public class CenterRepository : ICenterRepository
    {
        private readonly HttpClient _httpClient;

        public CenterRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CenterDto>> GetAllCenters()
        {

            List<CenterDto> centerDtos = new List<CenterDto>();
            List<PartDto> partDtos = new List<PartDto>();

            var response = await _httpClient.GetAsync("Center/GetAllCenters");

            var content = await response.Content.ReadAsStringAsync();
            centerDtos = GetcenterDtoFromContent(content);
            return centerDtos;
        }

        private static List<CenterDto> GetcenterDtoFromContent(string content)
        {
            List<CenterDto> centerDtos = new List<CenterDto>();
            JArray jsonResponse = JArray.Parse(content);
            foreach(var item in jsonResponse)
            {
                CenterDto? dto = JsonConvert.DeserializeObject<CenterDto>(item.ToString());
                centerDtos.Add(dto);
            }
            return centerDtos;
        }

        public async Task<List<CenterDto>> GetCenterByFilters(Guid Id,
                                                              string centerNamefilter = "",
                                                              int centerIDfilter = 0,
                                                              string partNamefilter = "",
                                                              int partIDfilter = 0)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id ", Id.ToString());
            parameters.Add("CenterName ", centerNamefilter);
            parameters.Add("CenterID ", centerIDfilter.ToString());
            parameters.Add("PartName ", partNamefilter);
            parameters.Add("PartID ", partIDfilter.ToString());

            List<CenterDto> centerDtos = new List<CenterDto>();
            var request = QueryHelpers.AddQueryString("Center/GetCenterByOtherFilters", parameters);
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            centerDtos = GetcenterDtoFromContent(content);
            return centerDtos;
        }

        public async Task<List<CenterDto>> GetAllCenersByPage(string page, string pageSize)
        {
            List<CenterDto> centerDtos = new List<CenterDto>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("PageNumber", page);
            parameters.Add("PageSize", pageSize);
            string request = QueryHelpers.AddQueryString("Center/GetCentersByPage", parameters);
            var response = await _httpClient.GetAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            centerDtos = GetcenterDtoFromContent(content);
            return centerDtos;
        }

        public async Task CreateCenter(CreateCenterCommand createCenterCommand)
        {
            await SendRequest<CreateCenterCommand>(createCenterCommand, HttpMethod.Post, "Center/CreateCenter");
        }

        public async Task AddPart(AddPartCommand addPartCommand)
        {
            await SendRequest<AddPartCommand>(addPartCommand, HttpMethod.Put, "Center/AddPart");
        }

        public async Task DeletePart(DeletePartCommand deletePartCommand)
        {
            await SendRequest<DeletePartCommand>(deletePartCommand, HttpMethod.Put, "Center/DeletePart");
        }

        public async Task DeleteCenter(DeleteCenterCommand deleteCenterCommand)
        {
            await SendRequest<DeleteCenterCommand>(deleteCenterCommand, HttpMethod.Delete, "Center");
        }
        public async Task EditCenter(EditCenterCommand editCenterCommand)
        {
            await SendRequest<EditCenterCommand>(editCenterCommand, HttpMethod.Put, "Center/EditCenterName");

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
