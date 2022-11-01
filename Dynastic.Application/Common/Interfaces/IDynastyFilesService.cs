using Microsoft.AspNetCore.Http;

namespace Dynastic.Application.Common.Interfaces;

public interface IDynastyFilesService
{
    Task UploadPersonPicture(IFormFile formFile, Guid dynastyId, Guid personId);
}