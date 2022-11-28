using Azure;
using Azure.Search.Documents.Indexes;
using Dynastic.Application.Common;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Dynasties.Queries;

public class GetDynastiesQuery : IRequest<PaginatedList<DynastyBasicDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
}

public class GetDynastiesQueryHandler : IRequestHandler<GetDynastiesQuery, PaginatedList<DynastyBasicDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDynastiesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<DynastyBasicDto>> Handle(GetDynastiesQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}