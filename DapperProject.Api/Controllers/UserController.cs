using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DapperProject.Application.Interfaces;
using DapperProject.Core.Entities;

namespace DapperProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _models;

        public UserController(IUnitOfWork models)
        {
            _models = models;
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
