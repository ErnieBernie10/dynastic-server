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
        var ud = await context.UserDynasties
            .Where(u =>currentUserService.UserId.Equals(u.UserId) && u.DynastyId.Equals(request.DynastyId))
            .Include(ud => ud.Dynasty)
            .ThenInclude(d => d!.Members)
            .FirstOrDefaultAsync(cancellationToken);
        return ud!.Dynasty!.Members!;
    }
}
