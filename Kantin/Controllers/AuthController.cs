using AutoMapper;
using Kantin.Data;
using Kantin.Service.Models.Auth;
using Kantin.Service.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private KantinEntities _entities;
        private IMapper _mapper;

        public AuthController(KantinEntities entities, IMapper mapper)
        {
            _entities = entities;
            _mapper = mapper;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]Login login)
        {
            using (var userProvider = new AccountProvider(_entities))
            {
                var loginResult = await userProvider.Login(HttpContext.RequestServices, login);

                if (!loginResult.Success)
                    return Unauthorized(loginResult);

                return Ok(loginResult);
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]Register register)
        {
            using (var userProvider = new AccountProvider(_entities))
            {
                var loginResult = await userProvider.Register(HttpContext.RequestServices, register);

                if (!loginResult.Success)
                    return BadRequest(loginResult);

                return Ok(loginResult);
            }
        }
    }
}
