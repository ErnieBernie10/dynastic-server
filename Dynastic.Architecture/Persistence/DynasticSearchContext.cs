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

    public async Task<List<T>> SearchWithPagination<T>(string term, int page, int pageSize, CancellationToken cancellationToken = default) where T : ISearchable
    {
        return await Client.Search<T>(term)
            .Value
            .GetResultsAsync()
            .Skip(page - 1)
            .Take(pageSize)
            .Select(document => document.Document)
            .ToListAsync(cancellationToken);
    }
}