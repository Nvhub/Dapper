using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

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
        public async Task<User> LoginAsync(UserAuth userAuth)
        {
            try
            {
                var procedure = "login_user";
                var parameters = new DynamicParameters();
                var user = await _connection.QueryFirstOrDefaultAsync<User>(procedure, userAuth,commandType : CommandType.StoredProcedure);
                return user;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                //string sql = "SELECT * FROM users";
                var procedure = "get_all_or_single";
                var users = await _connection.QueryAsync<User>(procedure, commandType : CommandType.StoredProcedure);
                return users.ToList();
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
                //string sql = "SELECT * FROM users WHERE id=@Id";
                var parameter = new DynamicParameters();
                var procedure = "get_all_or_single";
                parameter.Add("Id", id);
                User user = await _connection.QueryFirstOrDefaultAsync<User>(procedure, parameter, commandType : CommandType.StoredProcedure);
                return user;
            }
            catch
            {
                return null;
            }
        }
        public async Task<IEnumerable<User>> AddAsync(User entity)
        {

                //string sql = "INSERT INTO users (firstName, lastName, userName, email) VALUES (@FirstName, @LastName, @UserName, @Email)";
                var procedure = "insert_user";
                await _connection.ExecuteAsync(procedure, entity, commandType : CommandType.StoredProcedure);
                return await GetAllAsync();
  
        }

        public async Task<bool> CreateTableAsync(string table_name)
        {
            try
            {
                //string sql = $"CREATE TABLE {table_name} (id int NOT NULL IDENTITY(1,1) PRIMARY KEY,userName varchar(255) NOT NULL ,firstName varchar(255) NOT NULL,lastName varchar(255) NOT NULL, email varchar(255) NOT NULL)";
                var procedure = "create_user_table";
                var users = await _connection.QueryAsync(procedure, commandType : CommandType.StoredProcedure);
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
                //string sql = "DELETE FROM users WHERE id=@Id";
                var procedure = "delete_user";
                var parameter = new DynamicParameters();
                parameter.Add("Id", id);
                int user = await _connection.ExecuteAsync(procedure, parameter, commandType: CommandType.StoredProcedure);
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
                //string sql = $"Drop Table {table_name}";
                var procedure = "drop_user_table";
                var user = await _connection.QueryAsync(procedure, commandType: CommandType.StoredProcedure);
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
                //string sql = "UPDATE users SET firstName=@FirstName, lastName=@LastName, userName=@UserName, email=@Email WHERE id=@Id";
                var procedure = "update_user";
                await _connection.ExecuteAsync(procedure, entity, commandType: CommandType.StoredProcedure);
                return await GetAsync(entity.Id);
            }
            catch
            {
                return null;
            }
        }
    }
}
