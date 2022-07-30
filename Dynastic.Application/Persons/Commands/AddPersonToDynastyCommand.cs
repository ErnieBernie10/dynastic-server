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
    private readonly IApplicationDbContext context;
    private readonly ICurrentUserService currentUserService;

    public AddPersonToDynastyCommandHandler(ICurrentUserService currentUserService, IApplicationDbContext context)
    {
        this.context = context;
        this.currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(AddPersonToDynastyCommand request, CancellationToken cancellationToken)
    {
        var dynasty = await context.Dynasties
            .Where(d => d.UserId!.Equals(currentUserService.UserId) && d.Id.Equals(request.DynastyId))
            .FirstOrDefaultAsync();
        if (dynasty is null)
        {
            throw new NotFoundException(request.DynastyId.ToString(), nameof(Dynasty));
        }
        dynasty.Members!.Add(new Person
        {
            Firstname = request.Firstname,
            Middlename = request.Middlename,
            Lastname = request.Lastname,
            FatherId = request.FatherId,
            MotherId = request.MotherId,
            BirthDate = request.BirthDate,
        });
        var updated = context.Dynasties.Update(dynasty);
        await context.SaveChangesAsync();
        return updated.Entity.Id;
    }
}
