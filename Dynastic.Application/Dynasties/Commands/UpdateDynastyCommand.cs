using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;

namespace Dynastic.Application.Dynasties.Commands;

public class UpdateDynastyCommandBody
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Motto { get; set; }
}

public class UpdateDynastyCommand : UpdateDynastyCommandBody, IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class UpdateDynastyCommandHandler : IRequestHandler<UpdateDynastyCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public UpdateDynastyCommandHandler (IApplicationDbContext context, IAccessService accessService)
    {
        this._context = context;
        this._accessService = accessService;
    }
    
    public async Task<Guid> Handle(UpdateDynastyCommand request, CancellationToken cancellationToken)
    {
        var dynasty = _accessService.GetUserDynasties()
            .FirstOrDefault(d => d.Id.Equals(request.Id));

        if (dynasty is null)
        {
            throw new NotFoundException(request.Id.ToString(), nameof(Dynasty));
        }

        dynasty.Name = request.Name;
        dynasty.Description = request.Description;
        dynasty.Motto = request.Motto;
        
        await _context.SaveChangesAsync(cancellationToken);

        return dynasty.Id;
    }
}