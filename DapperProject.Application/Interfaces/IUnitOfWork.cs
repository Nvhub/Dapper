namespace DapperProject.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }

    }
}
