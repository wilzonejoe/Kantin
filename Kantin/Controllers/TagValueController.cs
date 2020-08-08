using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Core.Exceptions.Models;
using Core.Models.Auth;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Providers;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kantin.Controllers.Tag
{
    [Route("api/[controller]")]
    public class TagValueController: Controller
    {
        private readonly KantinEntities _entities;
        public TagValueController(KantinEntities entities) { _entities = entities; }

        [HttpGet]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<TagValue>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get()
        {
            using var service = new TagValueProvider(_entities);
            var result = await service.GetAll(null);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TagValue))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            using var service = new TagValueProvider(_entities);
            var result = await service.Get(id);
            return Ok(result);
        }

        [HttpPost]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(TagValue))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post([FromBody]TagValue tags)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using var service = new TagValueProvider(_entities, accountIdentity);
            var result = await service.Create(tags);
            return Created($"api/tags/{result.Id}", result);
        }

        [HttpPut("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TagValue))]
        [ProducesResponseType((int)HttpStatusCode.Conflict, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Put(Guid id, [FromBody]TagValue tagValue)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using (var service = new TagValueProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, tagValue);
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accountIdentity = AccountIdentityService.GenerateAccountIdentityFromClaims(_entities, HttpContext.User.Claims);
            using var service = new TagValueProvider(_entities, accountIdentity);
            var result = await service.Delete(id);
            if (result)
                return NoContent();

            return NotFound();
        }
    }
}
