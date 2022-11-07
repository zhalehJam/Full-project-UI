using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Programs.Dto;
using Ticketing.Models.Tickets.Dto;
using Ticketing.Models.Tickets.Repository;
using Microsoft.AspNetCore.WebUtilities;

namespace Ticketing.Repository.Tickets
{
    public class TicketRepository : ITicketRepository
    {
        private readonly HttpClient _httpClient;

        public TicketRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<TicketDto>> GetAllTickets()
        {
            List<TicketDto> ticketList = new List<TicketDto>();
            var response = await _httpClient.GetAsync("Ticket/GetAllTickets");
            var content = await response.Content.ReadAsStringAsync();
            ticketList = GetTicketDtoFromContent(content);
            return ticketList;
        }

        public async Task<TicketDto> GetTicketById(Guid TickeId)
        {
            TicketDto? ticketList = new TicketDto();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", TickeId.ToString()); 
            string request = QueryHelpers.AddQueryString("Ticket/GetAllTicketsByPage", parameters);
            var response = await _httpClient.GetAsync("Ticket/GetAllTickets");
            var content = await response.Content.ReadAsStringAsync();
            ticketList = GetTicketDtoFromContent(content).FirstOrDefault();
            return ticketList;
        }
        private static List<TicketDto> GetTicketDtoFromContent(string content)
        {
            List<TicketDto> ticketDtos = new List<TicketDto>();
            JArray jsonResponse = JArray.Parse(content);

            foreach(var item in jsonResponse)
            {
                var dto = JsonConvert.DeserializeObject<TicketDto>(item.ToString());
                ticketDtos.Add(dto);
            }
            return ticketDtos;
        }

        public async Task<List<TicketDto>> GetAllTickets(string page, string pageSize)
        {
            List<TicketDto> ticketList = new List<TicketDto>();
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("PageNumber", page);
            parameters.Add("PageSize", pageSize);
            string request = QueryHelpers.AddQueryString("Ticket/GetAllTicketsByPage", parameters);
            var response = await _httpClient.GetAsync("Ticket/GetAllTickets");
            var content = await response.Content.ReadAsStringAsync();
            ticketList = GetTicketDtoFromContent(content);
            return ticketList;
        }
    }
}
