using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kantin.Data;
using Kantin.Data.Models.Tag;
using Kantin.Service.Models.Auth;
using Kantin.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Kantin.Controllers.Tag
{
    [Route("api/[controller]")]
    public class TagGroupController : Controller
    {
        private KantinEntities _entities;

        public TagGroupController(KantinEntities entities) { _entities = entities; }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var service = new TagGroupProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            using (var service = new TagGroupProvider(_entities))
            {
                var result = await service.Get(id);
                return Ok(result);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TagGroup tags)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new TagGroupProvider(_entities, accountIdentity))
            {
                var result = await service.Create(tags);
                return Created($"api/tags/{result.Id}", result);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]TagGroup tagGroup)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new TagGroupProvider(_entities, accountIdentity))
            {
                var result = await service.Update(id, tagGroup);
                return Ok(result);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accountIdentity = new AccountIdentity(HttpContext.User.Claims);
            using (var service = new TagGroupProvider(_entities, accountIdentity))
            {
                var result = await service.Delete(id);
                if (result)
                    return NoContent();

                return NotFound();
            }
        }
    }
}
