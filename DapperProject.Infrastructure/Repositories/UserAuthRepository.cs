using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
    }
}
