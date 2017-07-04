using amMiddle.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace amMiddle.Controllers
{
    [Route("api/amController")]
    public class amController : Controller
    {
        private readonly amContext _context;

        public amController(amContext context)
        {
            _context = context;

            if (_context.amModels.Count() == 0)
            {
                _context.amModels.Add(new amModel());
                _context.SaveChanges();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] amModel item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.amModels.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("Responses", new { id = item.amModelId }, item);
        }        
    }
}
