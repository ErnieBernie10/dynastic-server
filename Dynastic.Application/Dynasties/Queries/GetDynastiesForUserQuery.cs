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
    private readonly ICurrentUserService currentUserService;
    public GetDynastiesForUserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        this.currentUserService = currentUserService;
    }
    public async Task<List<Dynasty>> Handle(GetDynastiesForUserQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId.ToString();
        return await _context.Dynasties
            .Where(d => d.UserId!.Equals(currentUserId))
            .ToListAsync();
    }
}
