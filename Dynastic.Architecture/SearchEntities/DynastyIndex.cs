using Azure.Search.Documents.Indexes;
using Dynastic.Domain.Entities;

namespace Dynastic.Infrastructure.SearchEntities;

public class DynastyIndex
{
    [SimpleField(IsKey = true)] public string Id { get; init; }
    [SearchableField(IsSortable = true)]
    public string Name { get; set; } = default!;
    [SearchableField]
    public string? Description { get; set; }
    [SearchableField]
    public string? Motto { get; set; }
    [SearchableField]
    public List<PersonIndex> Members { get; set; } = new();
}