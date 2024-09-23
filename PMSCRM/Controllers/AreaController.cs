using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : Controller
    {
        AreaService _areaService;

        public AreaController(AreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpGet("GetAreas")]
        public ActionResult<List<Area>> GetAreas()
        {
            var areas = _areaService.GetAreas();
            if (areas == null || !areas.Any())
            {
                return NotFound("No areas found");
            } 
            return Ok(areas);
        }

        [HttpPost("AddArea")] 
        public ActionResult AddArea([FromBody]Area area)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _areaService.AddArea(area);
            if (success)
            {
                return Ok("Area was added");
            }
            return BadRequest("Failed to add adrea");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateArea(Guid id, [FromBody] Area area)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _areaService.UpdateArea(id, area);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update area");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteArea(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _areaService.DeleteArea(id);
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
