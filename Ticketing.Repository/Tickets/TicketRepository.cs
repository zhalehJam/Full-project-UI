﻿using Framework.Pagination;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;
<<<<<<< Updated upstream
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Collections.Specialized;
using Ticketing.Models.Tickets.Command;
using Ticketing.Models.Shared;
using Ticketing.Models.Programs.Command;
=======
using System.Text.Json;
using Ticketing.Models.Tickets.Command;
using Ticketing.Models.Tickets.Dto;
using Ticketing.Models.Tickets.Query;
using Ticketing.Models.Tickets.Repository;
>>>>>>> Stashed changes

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

        public async Task CreateNewTicket(CreateTicketCommand createTicketCommand)
        {
            await SendRequest<CreateTicketCommand>(createTicketCommand, HttpMethod.Post, "Ticket");
        }

        public async Task UpdateTicket(UpdateTicketCommand updateTicketCommand)
        {
            await SendRequest<UpdateTicketCommand>(updateTicketCommand, HttpMethod.Put, "Ticket");

        }

        public async Task DeleteTicket(DeleteTicketCommand deleteTicketCommand)
        {
            await SendRequest<DeleteTicketCommand>(deleteTicketCommand, HttpMethod.Delete, "Ticket");
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

