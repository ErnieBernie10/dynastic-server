using Azure.Search.Documents.Indexes;

namespace Dynastic.Infrastructure.SearchEntities;

public class PersonIndex
{
    [SimpleField]
    public string Id { get; set; }
    [SearchableField]
    public string? Firstname { get; set; }
    [SearchableField]
    public string? MiddleName { get; set; }
    [SearchableField]
    public string? Lastname { get; set; }
    [SimpleField]
    public string? MotherId { get; set; }
    [SimpleField]
    public string? FatherId { get; set; }
}