using Dynastic.Domain.Entities;

namespace Dynastic.Application.Dynasties.Queries;

public class DynastyDto : Base
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Motto { get; set; }
    public List<Person> Members { get; set; } = new();
    public CreationStep CreationStep { get; set; }
    public bool IsPrimary { get; set; } = false;
    public string CoaPath { get; set; } = default!;
}