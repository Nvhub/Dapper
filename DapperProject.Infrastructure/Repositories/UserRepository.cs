using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperProject.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _sql_connection;
        private readonly SqlConnection _connection;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _sql_connection = configuration.GetConnectionString("DataBaseConnection");
            _connection = new SqlConnection(_sql_connection);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            { 
                string sql = "SELECT * FROM users";
                var users = await _connection.QueryAsync<User>(sql);
                return users;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User> GetAsync(int id)
        {
            try
            {
                string sql = "SELECT * FROM users WHERE id=@Id";
                User user = await _connection.QueryFirstOrDefaultAsync<User>(sql, new {Id = id});
                return user;
            }
            catch
            {
                return null;
            }
        }
        public async Task<IEnumerable<User>> AddAsync(User entity)
        {
            try
            {
                string sql = "INSERT INTO users (firstName, lastName, userName, email) VALUES (@FirstName, @LastName, @UserName, @Email)";
                await _connection.ExecuteAsync(sql, entity);
                return await GetAllAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateTableAsync(string table_name)
        {
            try
            {
                string sql = $"CREATE TABLE {table_name} (id int NOT NULL IDENTITY(1,1) PRIMARY KEY,userName varchar(255) NOT NULL ,firstName varchar(255) NOT NULL,lastName varchar(255) NOT NULL, email varchar(255) NOT NULL)";
                var users = await _connection.QueryAsync(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> DeleteAsync(int id)
        {
            try
            {
                string sql = "DELETE FROM users WHERE id=@Id";
                int user = await _connection.ExecuteAsync(sql, new { Id = id });
                return await GetAllAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DropTableAsync(string table_name)
        {
            try
            {
                string sql = $"Drop Table {table_name}";
                var user = await _connection.QueryAsync(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> UpdateAsync(User entity)
        {
            try
            {
                var user = await GetAsync(entity.Id);
                if(user == null)
                    return null;
                string sql = "UPDATE users SET firstName=@FirstName, lastName=@LastName, userName=@UserName, email=@Email WHERE id=@Id";
                await _connection.ExecuteAsync(sql, entity);
                return await GetAsync(entity.Id);
            }
            catch
            {
                return null;
            }
        }
    }
}
