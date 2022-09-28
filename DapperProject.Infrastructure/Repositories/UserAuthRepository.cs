using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DapperProject.Infrastructure.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly IUnitOfWork _models;
        private readonly IConfiguration _configuration;

        public UserAuthRepository(IUnitOfWork models, IConfiguration configuration)
        {
            _models = models;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> JsonWebTokenGenerate(UserAuth entity)
        {
            if (entity == null)
                return null;
            var user = await _models.Users.LoginAsync(entity);
            if (user == null)
                return null;

            var clamis = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: clamis,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: signIn);
            return token;


        }

        public async Task<UserLogin> GetUserLoginAsync(ClaimsIdentity identity)
        {
            //var identity = claims;

            if (identity != null)
            {
                var userClaim = identity.Claims;
                var user = new UserLogin()
                {
                    Id = userClaim.FirstOrDefault(user => user.Type == ClaimTypes.NameIdentifier)?.Value,
                    FullName = userClaim.FirstOrDefault(user => user.Type == ClaimTypes.Name)?.Value,
                    Email = userClaim.FirstOrDefault(user => user.Type == ClaimTypes.Email)?.Value,
                };
                return user;
            }
            return null;
        }
    }
}
