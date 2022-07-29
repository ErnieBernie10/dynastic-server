using Ardalis.GuardClauses;
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

namespace Dynastic.Application.Persons.Queries;

public class GetPersonsByDynastyQuery : IRequest<List<Person>>
{
    public Guid DynastyId { get; set; }
}

public class GetPersonsByDynastyQueryHandler : IRequestHandler<GetPersonsByDynastyQuery, List<Person>>
{
    private readonly IApplicationDbContext context;
    private readonly ICurrentUserService currentUserService;

    public GetPersonsByDynastyQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        this.context = context;
        this.currentUserService = currentUserService;
    }

    public async Task<List<Person>> Handle(GetPersonsByDynastyQuery request, CancellationToken cancellationToken)
    {
        var dynasty = await context.Dynasties
            .Where(d => d.UserId!.Equals(currentUserService.UserId) && d.Id.Equals(request.DynastyId))
            .FirstOrDefaultAsync();

        return dynasty?.Members is null ? throw new NotFoundException(nameof(Dynasty), request.DynastyId.ToString()) : dynasty.Members;
    }
}
