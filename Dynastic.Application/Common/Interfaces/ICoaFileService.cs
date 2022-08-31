using Microsoft.AspNetCore.Http;

namespace Dynastic.Application.Common.Interfaces;

public interface ICoaFileService
{
    Task UploadUserCoa(IFormFile requestCoa, Guid dynastyIc);
}