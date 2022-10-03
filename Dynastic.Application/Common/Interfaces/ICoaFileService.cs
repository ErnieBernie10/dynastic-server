using Dynastic.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Dynastic.Application.Common.Interfaces;

public interface ICoaFileService
{
    Task UploadUserCoa(IFormFile requestCoa, Guid dynastyIc);

    bool IsValidCoaSvg(Stream coaFileStream);
    bool HasCoaUploaded(Dynasty dynasty);
}