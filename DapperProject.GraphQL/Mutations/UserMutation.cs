using DapperProject.Core.Entities;
using DapperProject.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DapperProject.GraphQL.Mutations
{
    public class UserMutation
    {
        public async Task<IEnumerable<User>> AddUser([Service] IUserRepository userRepository,
            string userName, string firstName, string lastName, string email, string password)
        {
            User user = new User()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            IEnumerable<User> users = await userRepository.AddAsync(user);
            return users;
        }

        public async Task<User> UpdateUser([Service] IUserRepository userRepository,
            int id,string? userName, string? firstName, string? lastName, string? email, string? password)
        {
            var userFind = await userRepository.GetAsync(id);
            if (userFind == null)
                throw new Exception($"user with id {id} not found");
            User user = new User()
            {
                Id = id,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
 
            User users = await userRepository.UpdateAsync(user);
            return users;
        }

        public async Task<IEnumerable<User>> DeleteUser([Service] IUserRepository userRepository, int id)
        {
            var user = await userRepository.GetAsync(id);
            if (user == null)
                throw new Exception($"user with id {id} not found");
            var users = await userRepository.DeleteAsync(id);

            return users;
        }
    }
}
