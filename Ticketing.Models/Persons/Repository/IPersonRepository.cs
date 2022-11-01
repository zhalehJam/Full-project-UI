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
        Task<PersonDto> GetPersonById(Guid Id);
    }
}
