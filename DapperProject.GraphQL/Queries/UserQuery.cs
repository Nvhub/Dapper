using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace DapperProject.GraphQL.Queries
{
    public class UserQuery
    {
        public async Task<IEnumerable<User>> GetAllUsers([Service] IUserRepository userRepository)
        {
            var users = await userRepository.GetAllAsync();
            return users;
        }

        public async Task<User> GetUser([Service] IUserRepository userRepository, int id)
        {
            var user = await userRepository.GetAsync(id);
            if (user == null)
                throw new Exception($"user with id {id} not found");
            return user;
        }

        public async Task<string> GetAuthToken([Service] IUserAuthRepository userAuthRepository,
            string userName, string password)
        {
            UserAuth auth = new UserAuth()
            {
                UserName = userName,
                Password = password
            };
     
            var token = await userAuthRepository.JsonWebTokenGenerate(auth);
            if (token == null)
                throw new Exception($"username or password not valid");

            var tokenGenerate =   new JwtSecurityTokenHandler().WriteToken(token);

            return tokenGenerate;
        }
    }
}
