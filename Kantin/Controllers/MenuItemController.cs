using System.Threading.Tasks;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Kantin.Controllers
{
    [Route("api/[controller]")]
    public class MenuItemController : Controller
    {
        private KantinEntities _entities;

        public MenuItemController(KantinEntities entities) { _entities = entities; }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.GetAll(null);
                return Ok(result);
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.Get(id);
                return Ok(result);
            }
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]MenuItem menuItem)
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.CreateAsync(menuItem);
                return Created($"api/menuItem/{result.Id}", result);
            }
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]MenuItem menuItem)
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.UpdateAsync(id, menuItem);
                return Ok(result);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (var service = new MenuItemsProvider(_entities))
            {
                var result = await service.Delete(id);
                if (result)
                    return NoContent();
                
                return NotFound();
            }
        }
    }
}
