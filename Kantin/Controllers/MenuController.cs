using AutoMapper;
using Core.Exceptions.Models;
using Core.Models.Auth;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Models.Request;
using Kantin.Models.Response;
using Kantin.Service.Attributes;
using Kantin.Service.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private KantinEntities _entities;
        private IMapper _mapper;

        public MenuController(KantinEntities entities, IMapper mapper) 
        { 
            _entities = entities;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Menu>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get()
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EditableMenuResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.Get(id);
                var response = _mapper.Map<EditableMenuResponse>(result);
                return Ok(response);
            }
        }

        [HttpPost]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(EditableMenuResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post([FromBody]EditableMenuRequest editableMenu)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuProvider(_entities, accountIdentity))
            {
                var menu = _mapper.Map<Menu>(editableMenu);
                var result = await service.Create(menu);
                var response = _mapper.Map<EditableMenuResponse>(result);
                return Created($"api/menu/{result.Id}", response);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EditableMenuResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Put(Guid id, [FromBody]EditableMenuRequest editableMenu)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuProvider(_entities, accountIdentity))
            {
                var menu = _mapper.Map<Menu>(editableMenu);
                var result = await service.Update(id, menu);
                var response = _mapper.Map<EditableMenuResponse>(result);
                return Ok(response);
            }
        }

        [HttpDelete("{id}")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuProvider(_entities, accountIdentity))
            {
                var result = await service.Delete(id);
                if (result)
                    return NoContent();

                return NotFound();
            }
        }
    }
}
