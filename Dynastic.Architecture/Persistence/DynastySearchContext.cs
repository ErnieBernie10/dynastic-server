using Azure;
using Azure.Search.Documents;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using Dynastic.Infrastructure.SearchEntities;
using Mapster;

namespace Dynastic.Infrastructure.Persistence;

public class DynastySearchContext : IDynastySearchContext
{
    private SearchClient _client;

    public DynastySearchContext(SearchClient client)
    {
        _client = client;
    }

    public async Task<List<Dynasty>> SearchWithPagination(string? term, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(term))
        {
            term = "*";
        }

        return await _client
            .Search<DynastyIndex>(term, new SearchOptions() { Skip = (page - 1) * pageSize, Size = pageSize, })
            .Value
            .GetResultsAsync()
            .Select(document => new Dynasty() {
                Id = new Guid(document.Document.Id),
                Name = document.Document.Name,
                Description = document.Document.Description,
                Members = document.Document.Members.Select(m => new Person {
                    Id = new Guid(m.Id),
                    Firstname = m.Firstname,
                    Lastname = m.Lastname,
                    MiddleName = m.MiddleName,
                    FatherId = m.FatherId is not null ? new Guid(m.FatherId) : null,
                    MotherId = m.MotherId is not null ? new Guid(m.MotherId) : null,
                }).ToList(),
                Motto = document.Document.Motto,
            })
            .ToListAsync(cancellationToken);
    }
}