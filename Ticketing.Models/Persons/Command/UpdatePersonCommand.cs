using Ticketing.Client.Contracts.Persons;

namespace Ticketing.Models.Persons.Command
{
    public class UpdatePersonCommand
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } 
        public Guid PartId { get; set; }
        public RoleType personRole { get; set; }
    }
}
