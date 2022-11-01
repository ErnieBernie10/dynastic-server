using Dynastic.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Dynastic.Application.Persons.Commands;

public class AddPersonPictureBody
{
    public IFormFile Picture { get; set; }
}

public class AddPersonPictureCommand : AddPersonPictureBody, IRequest<Guid>
{
    public Guid PersonId { get; set; }
    public Guid DynastyId { get; set; }
}

public class AddPersonPictureCommandHandler: IRequestHandler<AddPersonPictureCommand, Guid>
{
    private readonly IDynastyFilesService _filesService;

    public AddPersonPictureCommandHandler(IDynastyFilesService filesService)
    {
        _filesService = filesService;
    }

    public Task<Guid> Handle(AddPersonPictureCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement
        throw new NotImplementedException();
    }
}