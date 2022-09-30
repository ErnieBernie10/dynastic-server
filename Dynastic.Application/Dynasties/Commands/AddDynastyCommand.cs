﻿using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Application.Dynasties.Commands;

public class AddDynastyCommand : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Motto { get; set; }
}

public class AddDynastyCommandHandler : IRequestHandler<AddDynastyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAccessService _accessService;

    public AddDynastyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IAccessService accessService)
    {
        this._context = context;
        this._currentUserService = currentUserService;
        this._accessService = accessService;
    }

    public async Task<Guid> Handle(AddDynastyCommand request, CancellationToken cancellationToken)
    {
        var unfinishedDynasty = _accessService.FilterUserDynasties(_context.Dynasties)
            .FirstOrDefault(d => d.CreationStep != CreationStep.Finalized);

        if (unfinishedDynasty is not null)
        {
            // TODO: We should probably throw an exception instead, but this is fine for now.
            return unfinishedDynasty.Id;
        }
        
        var entity = await _context.Dynasties.AddAsync(
            new Dynasty {
                Name = request.Name,
                Description = request.Description,
                Motto = request.Motto,
                CreationStep = CreationStep.BasicInfo,
                OwnershipProperties = new DynastyOwnershipProperties() { OwnerUserId = _currentUserService.UserId }
            }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Entity.Id;
    }
}