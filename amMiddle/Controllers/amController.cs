using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using amMiddle.Models;

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

        #region Commented Sample Code
        //// GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //} 
        #endregion
    }
}
