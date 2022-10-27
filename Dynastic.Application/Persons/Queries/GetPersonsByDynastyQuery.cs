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
    private readonly IAccessService _accessService;

    public GetPersonsByDynastyQueryHandler(IAccessService accessService)
    {
        _accessService = accessService;
    }

    public async Task<List<Person>> Handle(GetPersonsByDynastyQuery request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.GetUserDynasties()
            .Where(d => d.Id.Equals(request.DynastyId))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return dynasty?.Members ?? throw new NotFoundException(request.DynastyId.ToString(), nameof(Dynasty));
    }
}
