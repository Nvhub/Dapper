using DapperProject.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace DapperProject.Application.Interfaces
{
    public interface IUserAuthRepository
    {
        Task<JwtSecurityToken> JsonWebTokenGenerate(UserAuth entity);
    }
}
