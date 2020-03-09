using System;
using System.Threading.Tasks;
using AutoMapper;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Attributes;
using Kantin.Service.Models.Auth;
using Kantin.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class AddOnItemController : Controller
    {
        private KantinEntities _entities;
        private IMapper _mapper;

        public AddOnItemController(KantinEntities entities, IMapper mapper) 
        { 
            _entities = entities;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var service = new AddOnItemsProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var service = new AddOnItemsProvider(_entities))
            {
                var result = await service.Get(id);
                return Ok(result);
            }
        }

        [HttpPost]
        [UserAuthorization]
        public async Task<IActionResult> Post([FromBody]AddOnItem addOnItem)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new AddOnItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Create(addOnItem);
                return Created($"api/addOnItem/{result.Id}", result);
            }
        }

        [HttpPut("{id}")]
        [UserAuthorization]
        public async Task<IActionResult> Put(Guid id, [FromBody]AddOnItem addOnItem)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new AddOnItemsProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, addOnItem);
                return Ok(result);
            }
        }

        [HttpDelete("{id}")]
        [UserAuthorization]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
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
