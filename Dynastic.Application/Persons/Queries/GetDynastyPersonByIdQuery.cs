using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynastic.Application.Persons.Queries;

public class GetDynastyPersonByIdQuery : IRequest<Person>
{
    public Guid DynastyId { get; set; }
    public Guid Id { get; set; }
}

public class GetDynastyPersonByIdQueryHandler : IRequestHandler<GetDynastyPersonByIdQuery, Person>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public GetDynastyPersonByIdQueryHandler(IApplicationDbContext context, IAccessService accessService)
    {
        this._context = context;
        _accessService = accessService;
    }

    public async Task<Person> Handle(GetDynastyPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.FilterUserDynasties(_context.Dynasties)
            .Where(d => d.Id.Equals(request.DynastyId))
            .ToListAsync(cancellationToken: cancellationToken);
        var person = dynasty
            .Select(d => d.Members.FirstOrDefault(m => m.Id.Equals(request.Id)))
            .FirstOrDefault();
        if (person is null) {
            throw new NotFoundException(request.Id.ToString(), nameof(Person));
        }
        return person;
    }
}

