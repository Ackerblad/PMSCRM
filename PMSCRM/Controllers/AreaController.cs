using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using System.Threading.Tasks;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    //[ApiController]
    public class AreaController : Controller
    {
        AreaService _areaService;

        public AreaController(AreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Area>> GetAll()
        {
            var areas = _areaService.GetAll();
            if (areas == null || !areas.Any())
            {
                return NotFound("No areas found");
            } 
            return Ok(areas);
        }

        //[HttpPost("Add")] 
        //public ActionResult Add([FromBody]Area area)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var success = _areaService.Add(area);
        //    if (success)
        //    {
        //        return Ok("Area was added");
        //    }
        //    return BadRequest("Failed to add adrea");
        //}
        [HttpPost]
        public ActionResult AddArea(Area area)
        {
            if(!ModelState.IsValid)
            {
                return View(area);
            }

            bool success = _areaService.Add(area);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add area");
            return View(area);
        }

        //[HttpPut("{id}")]
        //public ActionResult Update(Guid guid, [FromBody] Area area)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _areaService.Update(guid, area);
        //    if (success)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest("Failed to update area");
        //}

        [HttpGet("EditArea/{id}")]
        public IActionResult EditArea(Guid id)
        {
            var area = _areaService.GetById(id);
            if (area == null)
            {
                return NotFound("Area not found");
            }
            return View(area);
        }

        [HttpPost("EditArea/{id}")]
        public IActionResult EditArea(Guid id, Area updatedArea)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedArea);
            }

            bool success = _areaService.Update(id, updatedArea);
            if (success)
            {
                return RedirectToAction("ViewAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to update area.");
            return View("updatedArea");
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete(Guid guid)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _areaService.Delete(guid);
        //    if (success)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest("Failed to delete area");
        //}

        // GET: Task/DeleteTask/{id}
        [HttpGet("DeleteArea/{id}")]
        public IActionResult DeleteArea(Guid id)
        {
            var area = _areaService.GetById(id);
            if (area == null)
            {
                return NotFound("Area not found");
            }

            return View(area);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var area = _areaService.GetById(id);
            if (area == null)
            {
                return NotFound("Area not found.");
            }

            bool success = _areaService.Delete(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Area deleted successfully!";
                return RedirectToAction("ViewAreas");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete area.");
            return View("DeleteArea", area);
        }

        public IActionResult AddArea()
        {
            return View();
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("ViewAreas")]
        public IActionResult ViewAreas()
        {
            var areas = _areaService.GetAll();
            return View(areas);
        }
        [HttpGet("EditArea")]
        public IActionResult EditArea()
        {
            return View();
        }
        [HttpGet("DeleteArea")]
        public IActionResult DeleteArea()
        {
            return View();
        }

    }
}
