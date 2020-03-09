using System;
using System.Threading.Tasks;
using AutoMapper;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Models.Common;
using Kantin.Service.Attributes;
using Kantin.Service.Models.Auth;
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
        public async Task<IActionResult> Get()
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.Get(id);
                var response = _mapper.Map<EditableMenuItem>(result);
                return Ok(response);
            }
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<IActionResult> Post([FromBody]EditableMenuItem editableMenuItem)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuItemsProvider(_entities, accountIdentity))
            {
                var menuItem = _mapper.Map<MenuItem>(editableMenuItem);
                var result = await service.Create(menuItem);
                return Created($"api/menuItem/{result.Id}", result);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization]
        public async Task<IActionResult> Put(Guid id, [FromBody]MenuItem menuItem)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, menuItem);
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [UserAuthorization]
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
