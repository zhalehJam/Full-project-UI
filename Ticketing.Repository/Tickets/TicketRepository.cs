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
            var response = await _httpClient.GetAsync("https://localhost:44359/api/Ticket/GetAllTickets");
            var content = await response.Content.ReadAsStringAsync();
            ticketList = GetTicketDtoFromContent(content);
            return ticketList;
        }

        public Task<TicketDto> GetTicketById(Guid TickeId)
        {
            throw new NotImplementedException();
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

    }
}
