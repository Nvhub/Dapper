using DapperProject.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DapperProject.Application.Interfaces
{
    public interface IUserAuthRepository
    {
        Task<JwtSecurityToken> JsonWebTokenGenerate(UserAuth entity);

        Task<UserLogin> GetUserLoginAsync(ClaimsIdentity identity);
    }
}
