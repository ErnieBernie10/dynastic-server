using Dynastic.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Dynastic.Application.Services;

public class DynastyFileService : IDynastyFilesService
{
    private readonly IFileStorageConfiguration _fileStorageConfiguration;
    private readonly IHttpContextAccessor _contextAccessor;

    public DynastyFileService(IFileStorageConfiguration fileStorageConfiguration, IHttpContextAccessor contextAccessor)
    {
        _fileStorageConfiguration = fileStorageConfiguration;
        _contextAccessor = contextAccessor;
    }
    
    public Task UploadPersonPicture(IFormFile formFile, Guid dynastyId, Guid personId)
    {
        throw new NotImplementedException();
    }
}