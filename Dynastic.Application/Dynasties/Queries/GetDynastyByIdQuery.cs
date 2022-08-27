using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Dynasties.Queries;

public class GetDynastyByIdQuery : IRequest<Dynasty>
{
    public Guid Id { get; set; }
}

public class GetDynastyByIdQueryHandler : IRequestHandler<GetDynastyByIdQuery, Dynasty>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public GetDynastyByIdQueryHandler(IApplicationDbContext context,
        IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    public async Task<Dynasty> Handle(GetDynastyByIdQuery request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.FilterUserDynasties(_context.Dynasties)
            .FirstOrDefaultAsync(d => d.Id.Equals(request.Id), cancellationToken: cancellationToken);

        return dynasty ?? throw new NotFoundException(request.Id.ToString(), nameof(Dynasty));
    }
}