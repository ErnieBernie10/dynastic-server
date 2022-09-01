using Ardalis.GuardClauses;
using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Dynasties.Commands;

public class CoaFileCommand
{
    public IFormFile Coa { get; set; }
}

public class AddDynastyCoaFileCommand : CoaFileCommand, IRequest<Guid>
{
    public Guid Id { get; set; }
}

public class AddDynastyCoaCommandHandler : IRequestHandler<AddDynastyCoaFileCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IAccessService _accessService;
    private readonly ICoaFileService _coaFileService;

    public AddDynastyCoaCommandHandler(IApplicationDbContext context, IAccessService accessService,
        ICoaFileService coaFileService)
    {
        _context = context;
        _accessService = accessService;
        _coaFileService = coaFileService;
    }

    public async Task<Guid> Handle(AddDynastyCoaFileCommand request, CancellationToken cancellationToken)
    {
        var dynasty = await _accessService.FilterUserDynasties(_context.Dynasties)
            .FirstOrDefaultAsync((dynasty => dynasty.Id.Equals(request.Id)),
                cancellationToken: cancellationToken);

        Guard.Against.NotFound(request.Id, dynasty, nameof(dynasty));

        // TODO: Add validation for SVG file

        await _coaFileService.UploadUserCoa(request.Coa, dynasty.Id);

        return dynasty.Id;
    }
}