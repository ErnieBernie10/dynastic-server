using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Services;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Dynasties.Commands;

public class AddRelationshipBody
{
    public Guid PersonId { get; set; }
    public Guid PartnerId { get; set; }
}

public class AddRelationshipCommand : AddRelationshipBody, IRequest<Guid>
{
    public Guid DynastyId { get; set; }
}

public class AddRelationshipCommandHandler : IRequestHandler<AddRelationshipCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public AddRelationshipCommandHandler(IApplicationDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    public async Task<Guid> Handle(AddRelationshipCommand request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.GetUserDynasties()
            .FirstOrDefaultAsync(d => d.Id.Equals(request.DynastyId), cancellationToken: cancellationToken);

        if (dynasty is null)
        {
            throw new NotFoundException(request.DynastyId.ToString(), nameof(Dynasty));
        }

        var person = dynasty.Members.FirstOrDefault(m => m.Id.Equals(request.PersonId));
        var partner = dynasty.Members.FirstOrDefault(m => m.Id.Equals(request.PartnerId));

        if (person is null)
        {
            throw new NotFoundException(request.PersonId.ToString(), nameof(Person));
        }
        if (partner is null)
        {
            throw new NotFoundException(request.PartnerId.ToString(), nameof(Person));
        }

        var relationshipManager = new PersonRelationshipManager(dynasty);

        relationshipManager.PairPartner(person, partner);
        
         await _context.SaveChangesAsync(cancellationToken);

         return request.PersonId;
    }
}