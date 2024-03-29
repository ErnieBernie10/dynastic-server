using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;

namespace Dynastic.Application.Common.Interfaces;

public interface IDynastySearchContext
{
    public Task<PaginatedList<Dynasty>> SearchWithPagination(string? term, int page, int pageSize,
        CancellationToken cancellationToken = default);
}