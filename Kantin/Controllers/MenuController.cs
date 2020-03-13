using Core.Models.Auth;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Attributes;
using Kantin.Service.Providers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private KantinEntities _entities;

        public MenuController(KantinEntities entities) { _entities = entities; }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.Get(id);
                return Ok(result);
            }
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<IActionResult> Post([FromBody]Menu menu)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuProvider(_entities, accountIdentity))
            {
                var result = await service.Create(menu);
                return Created($"api/menu/{result.Id}", result);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization]
        public async Task<IActionResult> Put(Guid id, [FromBody]Menu menu)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new MenuProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, menu);
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [UserAuthorization]
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
