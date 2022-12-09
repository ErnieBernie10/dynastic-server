using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Dynasties.Commands;

public class RedeemDynastyInviteCommand : IRequest<Guid>
{
    public Guid InviteId { get; set; }
}

public class RedeemDynastyInviteCommandHandler : IRequestHandler<RedeemDynastyInviteCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RedeemDynastyInviteCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }


    public async Task<Guid> Handle(RedeemDynastyInviteCommand request, CancellationToken cancellationToken)
    {
        var invite = await _context.DynastyInvitations.FindAsync(request.InviteId);

        Guard.Against.NotFound(request.InviteId, nameof(DynastyInvitation));

        invite!.IsRedeemed = true;

        var dynasty = await _context.Dynasties.FindAsync(invite.DynastyId);

        var userInfo = await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(_currentUserService.UserId),
            cancellationToken: cancellationToken);

        dynasty!.Members.Add(new Person() {
            Firstname = userInfo!.Firstname,
            MiddleName = userInfo.MiddleName,
            Lastname = userInfo.Lastname,
            BirthDate = userInfo.BirthDate,
        });

        _context.Dynasties.Update(dynasty);
        _context.DynastyInvitations.Update(invite);

        await _context.SaveChangesAsync(cancellationToken);

        return dynasty.Id;
    }
}