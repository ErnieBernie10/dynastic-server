using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Dynasties.Queries;

public class GetDynastiesForUserQuery : IRequest<List<Dynasty>>
{
}

public class GetDynastiesForUserQueryHandler : IRequestHandler<GetDynastiesForUserQuery, List<Dynasty>>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public GetDynastiesForUserQueryHandler(IApplicationDbContext context,
        IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    public async Task<List<Dynasty>> Handle(GetDynastiesForUserQuery request, CancellationToken cancellationToken)
    {
        return await _accessService.FilterUserDynasties(_context.Dynasties)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}