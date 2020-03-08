using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private KantinEntities _entities;

        public MenuController(KantinEntities entities) { _entities = entities; }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.Get(id);
                return Ok(result);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Menu menu)
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.CreateAsync(menu);
                return Created($"api/menu/{result.Id}", result);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]Menu menu)
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.UpdateAsync(id, menu);
                return Ok(result);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var service = new MenuProvider(_entities))
            {
                var result = await service.Delete(id);
                if (result)
                    return NoContent();

                return NotFound();
            }
        }
    }
}
