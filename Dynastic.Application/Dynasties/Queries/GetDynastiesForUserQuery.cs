using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Services;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Dynasties.Queries;

public class GetDynastiesForUserQuery : IRequest<List<DynastyDto>>
{
    public bool isFinished { get; set; } = true;
}

public class GetDynastiesForUserQueryHandler : IRequestHandler<GetDynastiesForUserQuery, List<DynastyDto>>
{
    private readonly IAccessService _accessService;

    public GetDynastiesForUserQueryHandler(IAccessService accessService)
    {
        _accessService = accessService;
    }

    public async Task<List<DynastyDto>> Handle(GetDynastiesForUserQuery request, CancellationToken cancellationToken)
    {
        var dynasties = await _accessService.GetUserDynasties()
            .AsNoTracking()
            .Where(d => !request.isFinished || d.CreationStep == CreationStep.Finalized)
            .ToListAsync(cancellationToken: cancellationToken);
        return dynasties.Adapt<List<DynastyDto>>();
    }
}