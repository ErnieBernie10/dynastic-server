using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Common.Messaging;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dynastic.Application.Dynasties.Commands;

public class InviteToDynastyCommand : IRequest<bool>
{
    public Guid DynastyId { get; set; }
    public string Email { get; set; }
}

public class InviteToDynastyCommandHandler : IRequestHandler<InviteToDynastyCommand, bool>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IServiceBus _serviceBus;
    private readonly IApplicationDbContext _context;

    public InviteToDynastyCommandHandler(IServiceBus serviceBus, IApplicationDbContext context,
        ICurrentUserService currentUserService)
    {
        _serviceBus = serviceBus;
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(InviteToDynastyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DynastyInvitations.AddAsync(
            new() { DynastyId = request.DynastyId, IsRedeemed = false, },
            cancellationToken);

        var currentUser =
            await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(_currentUserService.UserId),
                cancellationToken);
        var dynasty =
            await _context.Dynasties.FirstOrDefaultAsync((d) => d.Id.Equals(request.DynastyId), cancellationToken);

        Guard.Against.NotFound(nameof(Dynasty), request.DynastyId);

        if (currentUser is null)
        {
            throw new ArgumentException("User not defined");
        }

        // TODO: Store in some constant
        await _serviceBus.SendMessage("dynasty-email",
            JsonSerializer.Serialize(
                new InviteToDynastyMessage() {
                    Content =
                        $"{currentUser.Firstname} {currentUser.Lastname} invited you to their dynasty {dynasty?.Name}!{Environment.NewLine}{GetLink(entity.Entity.Id.ToString())}",
                    Subject = $"Invitation to {dynasty?.Name}",
                    To = request.Email,
                }, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    private string GetLink(string code)
    {
        // TODO: Construct link by taking info from http origin header or from some environment variable.
        return $"https://www.dynastic.be/invite?code={code}";
    }
}