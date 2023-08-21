using Framework.ReadModel.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Client.Contracts.Persons;

namespace Ticketing.Client.Contracts.Ticket.Queries
{
    public class GetPersonInfoByFiltersQuery: PaginationQueryParameters<GetPersonInfoByFiltersQuery>
    {
        public int PersonCode { get; set; }
        public Guid CenterId { get; set; }
        public Guid PartId { get; set; }
        public string PersonName { get; set; } = "";
        public string userRole{ get; set; }
    }
}
