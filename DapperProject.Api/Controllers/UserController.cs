using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;
using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Client;
using System.Text;

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

        public UserController(IUnitOfWork models, IMemoryCache cache, IRabbitMQRepository rabbitmqRepository)
        {
            _models = models;
            _cache = cache;
            _rabbitmqRepository = rabbitmqRepository;
        }

        [Route("cache")]
        [HttpGet]
        public async Task<IActionResult> MemoryCache()
        {
            if (_cache.TryGetValue(userListCacheKey, out IEnumerable<User> users))
            {
                Console.WriteLine("caching");
                return Ok(users);
            }
            else
            {
                users = await _models.Users.GetAllAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(50))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1024);
                _cache.Set(userListCacheKey, users, cacheEntryOptions);
                Console.WriteLine("not caching");
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

        [Route("{id?}")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrSingel(int id)
        {
            if (id != 0)
            {
                var user = await _models.Users.GetAsync(id);
                if(user != null)
                    return Ok(user);
                return NotFound($"user with id {id} not found");
            }
            return Ok(await _models.Users.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Add(User entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            IEnumerable<User> user = await _models.Users.AddAsync(entity);

            if (user == null)
                return BadRequest("data not valid");

            //var factory = new ConnectionFactory() { HostName = "localhost" };

            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //channel.QueueDeclare(queue: "sendMail", durable: false, exclusive: false, autoDelete: false, arguments: null);

            //var mail = Encoding.UTF8.GetBytes(entity.Email);

            //channel.BasicPublish(exchange: "", routingKey: "sendMail",  basicProperties: null, body: mail);


            _rabbitmqRepository.Publisher(entity.Email, "sendMail");
            return Ok(user);
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var user = await _models.Users.GetAsync(id);
            if (user == null)
                return NotFound($"user with id {id} not found");
            return Ok(await _models.Users.DeleteAsync(id));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User entity)
        {
            if (entity.Id == null || entity.Id == 0)
                return BadRequest("Id is required");
            var user = await _models.Users.GetAsync(entity.Id);
            if (user == null)
                return NotFound($"user with id {entity.Id} not found");
            return Ok(await _models.Users.UpdateAsync(entity));
        }
    }
}
