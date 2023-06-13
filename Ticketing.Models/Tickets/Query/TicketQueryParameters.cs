using Framework.ReadModel.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Models.Tickets.Query
{
    public class TicketQueryParameters: PaginationQueryParameters<TicketQueryParameters>
    {
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
}
