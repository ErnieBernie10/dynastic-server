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
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class AddDynastyCommandHandler : IRequestHandler<AddDynastyCommand, Guid>
{
    private IApplicationDbContext context;
    private ICurrentUserService currentUserService;

    public AddDynastyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        this.context = context;
        this.currentUserService = currentUserService;
    }
    public async Task<Guid> Handle(AddDynastyCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.UserDynasties.AddAsync(new UserDynasty
        {
            Dynasty = new Dynasty
            {
                Name = request.Name,
                Description = request.Description,
            },
            UserId = currentUserService.UserId
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entity.Entity.Dynasty!.Id;
    }
}
