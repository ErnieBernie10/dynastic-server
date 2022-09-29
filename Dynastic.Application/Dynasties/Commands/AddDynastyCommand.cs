using Dynastic.Application.Common.Interfaces;
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

    public AddDynastyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        this._context = context;
        this._currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(AddDynastyCommand request, CancellationToken cancellationToken)
    {
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