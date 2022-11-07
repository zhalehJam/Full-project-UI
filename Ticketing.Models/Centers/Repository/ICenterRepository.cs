using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Centers.Dto;

namespace Ticketing.Models.Centers.Repository
{
    public interface ICenterRepository
    {
        Task<List<CenterDto>> GetAllCenters();
        Task<List<CenterDto>> GetCenterByFilters(Guid Id,string centerNamefilter = "", int centerIDfilter = 0, string partNamefilter = "", int partIDfilter = 0);
        Task<List<CenterDto>> GetAllCenersByPage(string page,string pageSize);
    }
}
