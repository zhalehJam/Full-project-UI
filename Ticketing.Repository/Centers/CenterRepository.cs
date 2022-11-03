using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Centers.Dto;
using Ticketing.Models.Centers.Repository;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using System.Data;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

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
              
            var response = await _httpClient.GetAsync("https://localhost:44359/api/Center/GetAllCenters");

            var content = await response.Content.ReadAsStringAsync();
            centerDtos = GetcenterDtoFromContent( content);
            return centerDtos;
        }

        private static List<CenterDto> GetcenterDtoFromContent( string content)
        {
            List<CenterDto> centerDtos =new List<CenterDto>();
            JArray jsonResponse = JArray.Parse(content); 
            foreach(var item in jsonResponse)
            {
                CenterDto? dto = JsonConvert.DeserializeObject<CenterDto>(item.ToString());
                centerDtos.Add(dto);
            }
            return centerDtos;
        }

        public async Task<List<CenterDto>> GetCenterByFilters(string centerNamefilter = "",
                                                              int centerIDfilter = 0,
                                                              string partNamefilter = "",
                                                              int partIDfilter = 0)
        {
            string filters = "";
            //if(!string.IsNullOrWhiteSpace(centerNamefilter))
                filters += "CenterName=" + centerNamefilter;
            //if(centerIDfilter != 0)
                filters += "&CenterID=" + centerIDfilter;
            //if(!string.IsNullOrWhiteSpace(partNamefilter))
                filters += "&PartName=" + partNamefilter;
            //if(partIDfilter != 0)
                filters += "&PartID=" + partIDfilter;
            //if(filters.StartsWith('&'))
                //filters = filters.Substring(1);
            List<CenterDto> centerDtos = new List<CenterDto>();
            var response = await _httpClient.GetAsync("Https://localhost:44359/api/Center/" + "GetCenterByOtherFilters?"+filters);
            var content = await response.Content.ReadAsStringAsync();
            centerDtos = GetcenterDtoFromContent(content);
            return centerDtos;
        }
    }
}
