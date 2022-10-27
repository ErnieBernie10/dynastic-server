using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
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

        var relationship = dynasty.Relationships.FirstOrDefault(r =>
            r.PartnerId.Equals(request.PartnerId) && r.PersonId.Equals(request.PersonId) ||
            r.PartnerId.Equals(r.PersonId) || r.PersonId.Equals(r.PartnerId));

        if (relationship is not null)
        {
            throw new ArgumentException("Relationship already exists");
        }
        
        dynasty.Relationships.Add(new Relationship()
        {
            PersonId = request.PersonId,
            PartnerId = request.PartnerId
        });

         await _context.SaveChangesAsync(cancellationToken);

         return request.PersonId;
    }
}