using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dynastic.Application.Persons.Commands;

public class AddPersonToDynastyBody
{
    [Required]
    public string? Firstname { get; set; }
    public string? Middlename { get; set; }
    public string? Lastname { get; set; }
    public DateTime? BirthDate { get; set; }
    public Guid? MotherId { get; set; }
    public Guid? FatherId { get; set; }
}

public class AddPersonToDynastyCommand : AddPersonToDynastyBody, IRequest<Guid>
{
    public Guid DynastyId { get; set; } = Guid.Empty;
}

public class AddPersonToDynastyCommandHandler : IRequestHandler<AddPersonToDynastyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public AddPersonToDynastyCommandHandler(IApplicationDbContext context, IAccessService accessService)
    {
        this._context = context;
        _accessService = accessService;
    }

    public async Task<Guid> Handle(AddPersonToDynastyCommand request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.FilterUserDynasties(_context.Dynasties)
            .Where(d => d.Id.Equals(request.DynastyId))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (dynasty is null)
        {
            throw new NotFoundException(request.DynastyId.ToString(), nameof(Dynasty));
        }

        var person = new Person {
            Firstname = request.Firstname,
            Middlename = request.Middlename,
            Lastname = request.Lastname,
            FatherId = request.FatherId,
            MotherId = request.MotherId,
            BirthDate = request.BirthDate,
        };
        dynasty.Members!.Add(person);
        var updated = _context.Dynasties.Update(dynasty);
        await _context.SaveChangesAsync(cancellationToken);
        return person.Id;
    }
}
