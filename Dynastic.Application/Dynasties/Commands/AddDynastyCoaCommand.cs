using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Dynasties.Commands;

public class AddDynastyCoaCommand : IRequest<Guid>
{
    public Guid DynastyId { get; set; }
    public IFormFile Coa { get; set; }
    public string Configuration { get; set; }
}

public class AddDynastyCoaCommandHandler : IRequestHandler<AddDynastyCoaCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;

    public AddDynastyCoaCommandHandler(IApplicationDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    public async Task<Guid> Handle(AddDynastyCoaCommand request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.FilterUserDynasties(_context.Dynasties)
            .FirstOrDefaultAsync((dynasty => dynasty.Id.Equals(request.DynastyId)), cancellationToken: cancellationToken);
        if (dynasty is null)
        {
            throw new NotFoundException(request.DynastyId.ToString(), nameof(dynasty));
        }

        throw new NotImplementedException();
    }
}