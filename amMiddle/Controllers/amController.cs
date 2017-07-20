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

            if (_context.amModel.Count() == 0 && _context.amCapture.Count() == 0)
            {
                _context.amModel.Add(new amModel());
                _context.amCapture.Add(new amCapture());
                _context.SaveChanges();
            }
            else if (_context.amModel.Count() == 0 && _context.amCapture.Count() != 0)
            {
                _context.amModel.Add(new amModel());
                _context.SaveChanges();
            }
            else if (_context.amModel.Count() != 0 && _context.amCapture.Count() == 0)
            {
                _context.amCapture.Add(new amCapture());
                _context.SaveChanges();
            }
        }

        [HttpPost, Route("ProcessKeyLog")]
        public IActionResult ProcessKeyLog([FromBody] List<amModel> itemList)
        {
            if (itemList == null)
                return BadRequest();

            using (var db = new amContext())
            {
                db.DeleteDataSuccessSendToServer();

                itemList.ForEach(item => db.CreateData(item));
            }

            return CreatedAtRoute("SendMonitoringAsync", new { id = itemList[0].amModelId }, itemList);
        }

        [HttpPost, Route("ProcessCaptureImage")]
        public IActionResult ProcessCaptureImage([FromBody] List<amCapture> itemList)
        {
            if (itemList == null)
                return BadRequest();

            using (var db = new amContext())
            {
                db.DeleteImageSuccessSendToServer();

                itemList.ForEach(item => db.SaveImageData(item));
            }

            //return Created("ProcessCaptureImage", new { id = itemList[0].amCaptureId });
            return CreatedAtAction("SendScreenCapture", "amController", new { id = itemList[0].amCaptureId }, null);
            //return CreatedAtRoute("SendScreenCapture", new { id = itemList[0].amCaptureId }, null);
        }
    }
}
