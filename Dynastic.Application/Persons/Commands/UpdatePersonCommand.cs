using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Services;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dynastic.Application.Persons.Commands;

public class UpdatePersonBody
{
    [Required] public string? Firstname { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string? Lastname { get; set; }
    public DateTime? BirthDate { get; set; }
    public Guid? MotherId { get; set; }
    public Guid? FatherId { get; set; }
}
public class UpdatePersonCommand : UpdatePersonBody, IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid DynastyId { get; set; }
}

public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public UpdatePersonCommandHandler(IApplicationDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }
    public async Task<Guid> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.GetUserDynasties()
            .Where(d => d.Id.Equals(request.DynastyId))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (dynasty is null)
        {
            throw new NotFoundException(request.DynastyId.ToString(), nameof(Dynasty));
        }

        var person = dynasty.Members.FirstOrDefault(m => m.Id.Equals(request.Id));
        
        if (person is null)
        {
            throw new NotFoundException(request.Id.ToString(), nameof(Person));
        }

        var relationshipManager = new PersonRelationshipManager(dynasty);

        var newFather = dynasty.Members.FirstOrDefault(m => m.Id.Equals(request.FatherId));
        var newMother = dynasty.Members.FirstOrDefault(m => m.Id.Equals(request.MotherId));

        relationshipManager.UpdatePersonParents(person, newFather, newMother);

        person.Firstname = request.Firstname;
        person.Lastname = request.Lastname;
        person.MiddleName = request.MiddleName;
        person.BirthDate = request.BirthDate;

        await _context.SaveChangesAsync(cancellationToken);

        return person.Id;
    }
}