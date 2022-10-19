using Dynastic.Domain.Entities;

namespace Dynastic.Application.Common.Interfaces;

public interface IUserInfoService
{
    
    Task<UserInfo> GetUserInfo();
}