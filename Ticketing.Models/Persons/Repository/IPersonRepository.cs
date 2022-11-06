using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Persons.Dto;

namespace Ticketing.Models.Persons.Repository
{
    public interface IPersonRepository
    {
        Task<List<PersonDto>> GetAllPersons();
        Task<List<PersonDto>> GetAllPersons(int page, int pageSize);
        Task<PersonDto> GetPersonById(Guid Id);

    }
}
