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
    private readonly IApplicationDbContext context;
    private readonly ICurrentUserService currentUserService;

    public GetDynastyPersonByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        this.context = context;
        this.currentUserService = currentUserService;
    }

    public async Task<Person> Handle(GetDynastyPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var dynasty = await context.Dynasties
            .Where(d => d.UserId!.Equals(currentUserService.UserId))
            .Where(d => d.Id.Equals(request.DynastyId))
            .ToListAsync();
        var person = dynasty.Select(d => d.Members.FirstOrDefault(m => m.Id.Equals(request.Id)))
            .FirstOrDefault();
        if (person is null) {
            throw new NotFoundException(request.Id.ToString(), nameof(Person));
        }
        return person;
    }
}

