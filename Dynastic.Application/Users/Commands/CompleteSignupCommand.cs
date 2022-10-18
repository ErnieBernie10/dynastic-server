using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Users.Commands;

public class CompleteSignupCommand : IRequest<bool>
{
    public string? Firstname { get; set; }
    public string Middlename { get; set; }
    public string Lastname { get; set; }
    public DateTime BirthDate { get; set; }
}

public class CompleteSignupCommandHandler : IRequestHandler<CompleteSignupCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CompleteSignupCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(CompleteSignupCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(_currentUserService.UserId),
            cancellationToken: cancellationToken);

        if (user is not null)
        {
            return false;
        }

        _context.Users.Add(new UserInfo() {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Middlename = request.Middlename,
            BirthDate = request.BirthDate,
            UserId = _currentUserService.UserId,
        });
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}