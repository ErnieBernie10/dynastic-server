using Dynastic.Domain.Common.Interfaces;

namespace Dynastic.Application.Common.Interfaces;

public interface IDynasticSearchContext
{
    public Task<List<T>> SearchWithPagination<T>(string term, int page, int pageSize,
        CancellationToken cancellationToken = default) where T : ISearchable;
}