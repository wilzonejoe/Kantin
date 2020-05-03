using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class AddOnItemController : Controller
    {
        private KantinEntities _entities;

        public AddOnItemController(KantinEntities entities) 
        { 
            _entities = entities;
        }

        [HttpGet]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AddOnItem>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get([FromQuery]Query query)
        {
            using (var service = new AddOnItemsProvider(_entities))
            {
                var result = await service.GetAll(query);
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AddOnItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var service = new AddOnItemsProvider(_entities))
            {
                var result = await service.Get(id);
                return Ok(result);
            }
        }

        [HttpPost]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(AddOnItem))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post([FromBody]AddOnItem addOnItem)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new AddOnItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Create(addOnItem);
                return Created($"api/addOnItem/{result.Id}", result);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(AddOnItem))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Put(Guid id, [FromBody]AddOnItem addOnItem)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new AddOnItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, addOnItem);
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [UserAuthorization(nameof(Privilege.CanAccessMenu))]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new AddOnItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Delete(id);
                if (result)
                    return NoContent();

                return NotFound();
            }
        }
    }
}
