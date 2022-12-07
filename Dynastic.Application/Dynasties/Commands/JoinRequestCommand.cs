using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Common.Utils;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Common.Messaging;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dynastic.Application.Dynasties.Commands;

public class JoinRequestCommand : IRequest<bool>
{
    public Guid DynastyId { get; set; }
    public string Callback { get; set; }
}

public class JoinRequestCommandHandler : IRequestHandler<JoinRequestCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IServiceBus _serviceBus;

    public JoinRequestCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService,
        IServiceBus serviceBus)
    {
        _context = context;
        _currentUserService = currentUserService;
        _serviceBus = serviceBus;
    }

    public async Task<bool> Handle(JoinRequestCommand request, CancellationToken cancellationToken)
    {
        var dynasty =
            await _context.Dynasties.FirstOrDefaultAsync(d => d.Id.Equals(request.DynastyId), cancellationToken);

        Guard.Against.NotFound(request.DynastyId, typeof(Dynasty));

        var ownerId = dynasty?.OwnershipProperties.OwnerUserId!;

        var ownerInfo = await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(ownerId), cancellationToken);
        var currentUser =
            await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(_currentUserService.UserId),
                cancellationToken);

        var joinRequest = await _context.DynastyJoinRequests.AddAsync(new DynastyJoinRequest() {
            DynastyId = request.DynastyId, UserId = currentUser!.UserId, IsApproved = false, IsRedeemed = false,
        }, cancellationToken);

        var message = new EmailMessage() {
            Content =
                // TODO: Use proper html email template
                $"{currentUser.Firstname} {currentUser.Lastname} would like to join your dynasty {dynasty?.Name}!{Environment.NewLine} Accept: {GetCallback(request.Callback, joinRequest.Entity.Id.ToString(), true)} Deny: {GetCallback(request.Callback, joinRequest.Entity.Id.ToString(), false)} ",
            Subject = $"Request to join {dynasty!.Name}",
            To = ownerInfo!.Email,
        };

        // TODO: Store queue name in some constant
        await _serviceBus.SendMessage("dynasty-email",
            JsonSerializer.Serialize(message,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

        return true;
    }

    private string GetCallback(string callback, string code, bool approve)
    {
        if (!UrlValidation.IsValidEnvironment(callback))
        {
            throw new ArgumentException("Callback is not a valid link or is not pointing to a correct environment.");
        }

        var uri = new UriBuilder(callback) { Query = $"?code={code}&approved={approve.ToString().ToLowerInvariant()}" };
        return uri.Uri.ToString();
    }
}