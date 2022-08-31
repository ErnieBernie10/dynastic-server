using Dynastic.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Dynastic.Application.Services;

public class CoaFileService : ICoaFileService
{
    private readonly IFileStorageConfiguration _fileStorageConfiguration;

    public CoaFileService(IFileStorageConfiguration fileStorageConfiguration)
    {
        _fileStorageConfiguration = fileStorageConfiguration;
    }

    public async Task UploadUserCoa(IFormFile requestCoa, Guid dynastyId)
    {
        await using var fileStream =
            File.Create(Path.Combine(_fileStorageConfiguration.UserCoaEnvironmentPath(), dynastyId + ".svg"));

        await requestCoa.CopyToAsync(fileStream);
    }
}