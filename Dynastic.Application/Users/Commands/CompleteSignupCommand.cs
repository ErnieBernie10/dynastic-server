using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using Dynastic.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Users.Commands;

public class CompleteSignupCommand : IRequest<Guid>
{
    public string Firstname { get; set; }
    public string? MiddleName { get; set; }
    public string Lastname { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
}

public class CompleteSignupCommandHandler : IRequestHandler<CompleteSignupCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CompleteSignupCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CompleteSignupCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.Equals(_currentUserService.UserId),
            cancellationToken: cancellationToken);

        if (user is not null)
        {
            return user.Id;
        }

        var userInfo = await _context.Users.AddAsync(new UserInfo() {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            MiddleName = request.MiddleName,
            BirthDate = request.BirthDate,
            Email = request.Email,
            UserId = _currentUserService.UserId,
        }, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);

        return userInfo.Entity.Id;
    }
}