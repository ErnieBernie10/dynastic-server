using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Xml;

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
        await using var coaSvgStream = requestCoa.OpenReadStream();
        
        if (!IsValidCoaSvg(coaSvgStream))
        {
            throw new ArgumentException($"Coa file is invalid");
        }
        
        await using var fileStream =
            File.Create(Path.Combine(_fileStorageConfiguration.UserCoaEnvironmentPath(), dynastyId + ".svg"));

        await requestCoa.CopyToAsync(fileStream);
    }

    public bool IsValidCoaSvg(Stream coaFileStream)
    {
        // TODO: Expand this to check if all elements of the svg are valid. User could hijack the webapi and upload any svg they want.

        var svgString = coaFileStream.ReadBytesToString(256);

        return svgString.Contains("<svg ") || svgString.Contains("<svg\n") || svgString.Contains("<svg\r\n");
    }
}