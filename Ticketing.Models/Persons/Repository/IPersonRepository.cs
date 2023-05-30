using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Models.Persons.Command;
using Ticketing.Models.Persons.Dto;

namespace Ticketing.Models.Persons.Repository
{
    public interface IPersonRepository
    {
        Task<List<PersonDto>> GetAllPersons();
        Task<List<PersonDto>> GetAllPersons(string page, string pageSize);
        Task<PersonDto> GetPersonById(Guid Id);
        Task CreatePerson(CreatePersonCommand createPersonCommand);
        Task UpdatePerson(UpdatePersonCommand updatePersonCommand);
        Task DeletePerson(DeletePersonCommand deletePersonCommand);
        Task<PersonDto> GetPersonInfoByPersonelCode(int PersonnelCode);
    }
}
