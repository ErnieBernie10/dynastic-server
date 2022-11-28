using Dynastic.Domain.Entities;

namespace Dynastic.Application.Dynasties.Queries;

public class DynastyDto : DynastyBasicDto
{
    public List<Person> Members { get; set; } = new();
    public CreationStep CreationStep { get; set; }
    public bool IsPrimary { get; set; } = false;
}