using AutoMapper;
using Core.Exceptions.Models;
using Core.Model;
using Core.Models.Auth;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Attributes;
using Kantin.Service.Providers;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class StaffMemberController : Controller
    {
        private KantinEntities _entities;
        private IMapper _mapper;

        public StaffMemberController(KantinEntities entities, IMapper mapper)
        {
            _entities = entities;
            _mapper = mapper;
        }

        [HttpGet]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Account>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get([FromQuery]Query query)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new StaffMemberProvider(_entities, accountIdentity))
            {
                var result = await service.GetAll(query);
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Account))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new StaffMemberProvider(_entities, accountIdentity))
            {
                var result = await service.Get(id);
                return Ok(result);
            }
        }

        [HttpPost]
        [UserAuthorization(nameof(Privilege.CanAccessStaffMember))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Account))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post([FromBody]Account account)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new StaffMemberProvider(_entities, accountIdentity))
            {
                var result = await service.Create(account);
                return Created($"api/staffmember/{result.Id}", result);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization(nameof(Privilege.CanAccessStaffMember))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Account))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Put(Guid id, [FromBody]Account account)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new StaffMemberProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, account);
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [UserAuthorization(nameof(Privilege.CanAccessStaffMember))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new StaffMemberProvider(_entities, accountIdentity))
            {
                var result = await service.Delete(id);
                if (result)
                    return NoContent();

                return NotFound();
            }
        }
    }
}
