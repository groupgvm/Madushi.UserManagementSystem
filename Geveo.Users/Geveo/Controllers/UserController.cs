using Geveo.Users.Flow.Commands.Users;
using Microsoft.AspNetCore.Mvc;

namespace Geveo.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("CreateUser")]
        public async Task CreateUser([FromBody] CreateUserCommand command)
        {
            var email = command.Email;
        }

    }
}