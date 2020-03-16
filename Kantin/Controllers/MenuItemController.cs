﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class MenuItemController : Controller
    {
        private KantinEntities _entities;
        private IMapper _mapper;

        public MenuItemController(KantinEntities entities, IMapper mapper)
        {
            _entities = entities;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<MenuItem>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get()
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EditableMenuItemResponse))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.Get(id);
                var response = _mapper.Map<EditableMenuItemResponse>(result);
                return Ok(response);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Paging(int pageNumber, int pageSize)
        {
            var service = new MenuItemsProvider(_entities);
            var result = service.Paging(pageNumber, pageSize);
            return Ok(result);

        }

        [HttpPost]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(EditableMenuItemResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Post([FromBody]EditableMenuItemRequest editableMenuItem)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuItemsProvider(_entities, accountIdentity))
            {
                var menuItem = _mapper.Map<MenuItem>(editableMenuItem);
                var result = await service.Create(menuItem);
                var response = _mapper.Map<EditableMenuItemResponse>(result);
                return Created($"api/menuItem/{result.Id}", response);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization]
        [Produces(SwaggerConstant.JsonResponseType)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(EditableMenuItemResponse))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized, Type = typeof(ApiError))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiError))]
        public async Task<IActionResult> Put(Guid id, [FromBody]EditableMenuItemRequest editableMenuItem)
        {
            var menuItem = _mapper.Map<MenuItem>(editableMenuItem);
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, menuItem);
                var response = _mapper.Map<EditableMenuItemResponse>(result);
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
            using (var service = new MenuItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Delete(id);
                if (result)
                    return NoContent();
                
                return NotFound();
            }
        }
    }
}
