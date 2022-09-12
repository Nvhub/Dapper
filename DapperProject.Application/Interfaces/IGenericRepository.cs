namespace DapperProject.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> CreateTableAsync(string table_name);
        Task<bool> DropTableAsync(string table_name);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> DeleteAsync(int id);
    }
}
