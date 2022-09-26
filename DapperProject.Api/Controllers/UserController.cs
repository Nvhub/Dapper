using Microsoft.AspNetCore.Mvc;
using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using DapperProject.Api.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace DapperProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string userListCacheKey = "userList";
        private readonly IUnitOfWork _models;
        private readonly IMemoryCache _cache;
        private readonly IRabbitMQRepository _rabbitmqRepository;
        private readonly MemoryCacheUtils _memoryCacheUtils;
        private readonly IUserAuthRepository _userAuthRepository;

        public UserController(IUserAuthRepository userAuthRepository, IUnitOfWork models, IMemoryCache cache, IRabbitMQRepository rabbitmqRepository, MemoryCacheUtils memoryCacheUtils)
        {
            _models = models;
            _cache = cache;
            _rabbitmqRepository = rabbitmqRepository;
            _memoryCacheUtils = memoryCacheUtils;
            _userAuthRepository = userAuthRepository;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(UserAuth userAuth)
        {
            var user = await _userAuthRepository.JsonWebTokenGenerate(userAuth);
            if (user == null)
                return BadRequest("password or username not valid");
            return Ok(new JwtSecurityTokenHandler().WriteToken(user));
        }  

        [Route("{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrSingel(int id)
        {
            if (_cache.TryGetValue(userListCacheKey, out IEnumerable<User> users))
            {
                if (id != 0) {
                    var user = users.FirstOrDefault(user => user.Id == id);
                    if(user == null)
                        return NotFound("user not found");
                    Console.WriteLine("caching");
                    return Ok(user);
                }
                Console.WriteLine("caching");
                return Ok(users);
            }
            else
            {
                users = await _models.Users.GetAllAsync();
                var cacheEntryOptions = _memoryCacheUtils.memoryCacheEntryOptions();
                _cache.Set(userListCacheKey, users, cacheEntryOptions);
                Console.WriteLine("not caching");
                if (id != 0) {
                    var user = users.FirstOrDefault(user => user.Id == id);
                    if (user == null)
                        return NotFound("user not found");
                    return Ok(user);
                }
                return Ok(users);
            }
        }

        [Route("table/create")]
        [HttpPost]
        public async Task<IActionResult> CreateTable()
        {
            string table = "users";
            bool created = await _models.Users.CreateTableAsync(table);
            if (created)
                return Created("localhost", $"Table {table} created");
            else
                return BadRequest($"Table {table} isExist");
        }

        [Route("table/drop")]
        [HttpDelete]
        public async Task<IActionResult> DropTable()
        {
            string table = "users";
            bool created = await _models.Users.DropTableAsync(table);
            if (created)
                return Ok($"Table {table} deleted");
            else
                return BadRequest($"Table {table} not exist");
        }

        [HttpPost]
        public async Task<IActionResult> Add(User entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            IEnumerable<User> user = await _models.Users.AddAsync(entity);

            if (user == null)
                return BadRequest("data not valid");

            _rabbitmqRepository.Publisher(entity.Email, "sendMail");
            if(_cache.TryGetValue(userListCacheKey, out IEnumerable<User> users))
            {
                users.Append(entity);
                var cacheEntryOptions = _memoryCacheUtils.memoryCacheEntryOptions();
                _cache.Set(userListCacheKey, entity, cacheEntryOptions);
            }
 

            return Ok(user);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _models.Users.GetAsync(id);
            if (user == null)
                return NotFound($"user with id {id} not found");
            var deleted = await _models.Users.DeleteAsync(id);
            if(_cache.TryGetValue(userListCacheKey, out IEnumerable<User> users))
            {
                _cache.Set(userListCacheKey, deleted, _memoryCacheUtils.memoryCacheEntryOptions());
            }
            return Ok(deleted);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User entity)
        {
            if (entity.Id == null || entity.Id == 0)
                return BadRequest("Id is required");
            var user = await _models.Users.GetAsync(entity.Id);
            if (user == null)
                return NotFound($"user with id {entity.Id} not found");
            
            var updated = await _models.Users.UpdateAsync(entity);
            if (_cache.TryGetValue(userListCacheKey, out IEnumerable<User> users))
            {

                _cache.Set(userListCacheKey, updated, _memoryCacheUtils.memoryCacheEntryOptions());
            }
            return Ok(updated);
        }
    }
}
