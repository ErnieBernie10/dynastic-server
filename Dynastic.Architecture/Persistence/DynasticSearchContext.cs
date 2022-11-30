using Azure;
using Azure.Search.Documents;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;

namespace Dynastic.Infrastructure.Persistence;

public class DynasticSearchContext : IDynasticSearchContext
{
    public SearchClient Client { get; }

    public DynasticSearchContext(SearchClient client)
    {
        Client = client;
    }

    public async Task<List<T>> SearchWithPagination<T>(string? term, int page, int pageSize, CancellationToken cancellationToken = default) where T : ISearchable
    {
        if (string.IsNullOrEmpty(term))
        {
            term = "*";
        }

        return await Client.Search<T>(term, new SearchOptions() {
                Skip = page - 1,
                Size = pageSize,
            })
            .Value
            .GetResultsAsync()
            .Select(document => document.Document)
            .ToListAsync(cancellationToken);
    }
}