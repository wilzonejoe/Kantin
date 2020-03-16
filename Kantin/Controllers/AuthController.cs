using AutoMapper;
using Core.Exceptions.Models;
using Kantin.Data;
using Kantin.Service.Models.Auth;
using Kantin.Service.Providers;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
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
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(LoginResult))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Login([FromBody]Login login)
        {
            using (var userProvider = new AccountProvider(_entities))
            {
                var tokenService = HttpContext.RequestServices.GetService<ITokenAuthorizationService>();
                var loginResult = await userProvider.Login(tokenService, login);

                if (!loginResult.Success)
                    return Unauthorized(loginResult);

                return Ok(loginResult);
            }
        }

        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(LoginResult))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Register([FromBody]Register register)
        {
            using (var userProvider = new AccountProvider(_entities))
            {
                var tokenService = HttpContext.RequestServices.GetService<ITokenAuthorizationService>();
                var loginResult = await userProvider.Register(tokenService, register);

                if (!loginResult.Success)
                    return BadRequest(loginResult);

                return Ok(loginResult);
            }
        }
    }
}
