using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
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

        [HttpPost("Add")] 
        public ActionResult Add([FromBody]Area area)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _areaService.Add(area);
            if (success)
            {
                return Ok("Area was added");
            }
            return BadRequest("Failed to add adrea");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] Area area)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _areaService.Update(guid, area);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update area");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _areaService.Delete(guid);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete area");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
