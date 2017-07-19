using amMiddle.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

            if (_context.amModel.Count() == 0)
            {
                _context.amModel.Add(new amModel());
                _context.SaveChanges();
            }
        }

        [HttpPost, Route("Process")]
        public IActionResult Process([FromBody] List<amModel> itemList)
        {
            if (itemList == null)
                return BadRequest();

            using (var db = new amContext())
            {
                db.DeleteDataSuccessSendToServer();

                itemList.ForEach(item => db.CreateData(item));                
            }

            return CreatedAtRoute("CreateMonitoringAsync", new { id = itemList[0].amModelId }, itemList);
            //return CreatedAtRoute("Responses", new { id = item.amModelId }, null);
        }
    }
}
