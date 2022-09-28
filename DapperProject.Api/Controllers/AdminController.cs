using DapperProject.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DapperProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IUserAuthRepository _userAuthRepository;

        public AdminController(IUserAuthRepository userAuthRepository)
        {
            _userAuthRepository = userAuthRepository;
        }

        public class UserLogin
        {
            public string Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var user = GetUser();
            var user = await _userAuthRepository.GetUserLoginAsync(HttpContext.User.Identity as ClaimsIdentity);
            return Ok($" Welcome To Admin Api\n ID : {int.Parse(user.Id)}\n FullName : {user.FullName}\n Email : {user.Email}");
        }

        private UserLogin GetUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaim = identity.Claims;
                var user = new UserLogin()
                {
                    Id = userClaim.FirstOrDefault(user => user.Type == ClaimTypes.NameIdentifier)?.Value,
                    FullName = userClaim.FirstOrDefault(user => user.Type == ClaimTypes.Name)?.Value,
                    Email = userClaim.FirstOrDefault(user => user.Type == ClaimTypes.Email)?.Value,
                };
                return user;
            }
            return null;
        }
    }
}
