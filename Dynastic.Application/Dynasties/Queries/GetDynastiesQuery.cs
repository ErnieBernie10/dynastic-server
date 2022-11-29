using Azure;
using Dynastic.Application.Common;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Dynasties.Queries;

public class GetDynastiesQuery : IRequest<List<DynastyBasicDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string Search { get; set; } = default!;
}

public class GetDynastiesQueryHandler : IRequestHandler<GetDynastiesQuery, List<DynastyBasicDto>>
{
    private readonly IDynasticSearchContext _dynasticSearch;

    public GetDynastiesQueryHandler(IDynasticSearchContext dynasticSearch)
    {
        _dynasticSearch = dynasticSearch;
    }

    public async Task<List<DynastyBasicDto>> Handle(GetDynastiesQuery request,
        CancellationToken cancellationToken)
    {
        var response =
            await _dynasticSearch.SearchWithPagination<Dynasty>(request.Search, request.Page, request.PageSize,
                cancellationToken);

        return response.Adapt<List<DynastyBasicDto>>();
    }
}