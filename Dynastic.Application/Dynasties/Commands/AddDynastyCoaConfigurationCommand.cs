using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Dynastic.Application.Dynasties.Commands;

public class AddDynastyCoaConfigurationBody
{
    public JsonDocument CoaConfiguration { get; set; }
}

public class AddDynastyCoaConfigurationCommand : AddDynastyCoaConfigurationBody, IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class AddDynastyCoaConfigurationCommandHandler : IRequestHandler<AddDynastyCoaConfigurationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public AddDynastyCoaConfigurationCommandHandler(IApplicationDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    public async Task<Guid> Handle(AddDynastyCoaConfigurationCommand request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.FilterUserDynasties(_context.Dynasties)
            .FirstOrDefaultAsync(dynasty => dynasty.Id.Equals(request.Id), cancellationToken: cancellationToken);

        Guard.Against.NotFound(request.Id, dynasty, nameof(request));

        dynasty.CoaConfiguration = request.CoaConfiguration;
        dynasty.CreationStep = CreationStep.Coa;

        await _context.SaveChangesAsync(cancellationToken);

        return dynasty.Id;
    }
}