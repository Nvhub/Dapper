using DapperProject.Core.Entities;

namespace DapperProject.Application.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> LoginAsync(UserAuth userAuth);
    }
}
