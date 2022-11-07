using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Programs.Dto;
using Ticketing.Models.Tickets.Dto;

namespace Ticketing.Models.Tickets.Repository
{
    public interface ITicketRepository
    {
        Task<List<TicketDto>> GetAllTickets();
        Task<List<TicketDto>> GetAllTickets(string page, string pageSize);
        Task<TicketDto> GetTicketById(Guid TickeId);
    }
}
