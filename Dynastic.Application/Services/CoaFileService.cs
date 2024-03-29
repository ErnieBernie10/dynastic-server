using Dynastic.Application.Common.Interfaces;
using Dynastic.Application.Dynasties.Queries;
using Dynastic.Domain.Entities;
using Dynastic.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Xml;

namespace Dynastic.Application.Services;

public class CoaFileService : ICoaFileService
{
    private readonly IFileStorageConfiguration _fileStorageConfiguration;
    private readonly IHttpContextAccessor _contextAccessor;

    public CoaFileService(IFileStorageConfiguration fileStorageConfiguration, IHttpContextAccessor contextAccessor)
    {
        _fileStorageConfiguration = fileStorageConfiguration;
        _contextAccessor = contextAccessor;
    }

    private string GetDynastyCoaFilePath(string dynastyId)
    {
        return Path.Combine(_fileStorageConfiguration.UserCoaEnvironmentPath(), dynastyId + ".svg");
    }

    public async Task UploadUserCoa(IFormFile requestCoa, Guid dynastyId)
    {
        await using var coaSvgStream = requestCoa.OpenReadStream();
        
        if (!IsValidCoaSvg(coaSvgStream))
        {
            throw new ArgumentException($"Coa file is invalid");
        }
        
        await using var fileStream =
            File.Create(GetDynastyCoaFilePath(dynastyId.ToString()));

        await requestCoa.CopyToAsync(fileStream);
    }

    public bool IsValidCoaSvg(Stream coaFileStream)
    {
        // TODO: Expand this to check if all elements of the svg are valid. User could hijack the webapi and upload any svg they want.

        var svgString = coaFileStream.ReadBytesToString(256);

        return svgString.Contains("<svg ") || svgString.Contains("<svg\n") || svgString.Contains("<svg\r\n");
    }

    public bool HasCoaUploaded(Dynasty dynasty)
    {
        return File.Exists(GetDynastyCoaFilePath(dynasty.Id.ToString()));
    }

    public string GetCoaPath(Dynasty dynasty)
    {
        var protocol = _contextAccessor.HttpContext.Request.IsHttps ? "https" : "http";
        return $"{protocol}://{_contextAccessor.HttpContext.Request.Host.Value}{_fileStorageConfiguration.CoaFileServePath}/{dynasty.Id}.svg";
    }
}