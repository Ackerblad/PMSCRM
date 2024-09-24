using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessController : Controller
    {
        ProcessService _processService;

        public ProcessController(ProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Process>> GetAll()
        {
            var process = _processService.GetAll();
            if (process == null || !process.Any())
            {
                return NotFound("No process found");
            }

            return Ok(process);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] Process process)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _processService.Add(process);
            if (success)
            {
                return Ok("Process was added");
            }
            return BadRequest("Failed to add process");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] Process process)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _processService.Update(guid, process);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update process");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _processService.Delete(guid);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete process");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
