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

        [HttpGet("GetProcesses")]
        public ActionResult<List<Process>> GetProcesses()
        {
            var process = _processService.GetProcesses();
            if (process == null || !process.Any())
            {
                return NotFound("No process found");
            }

            return Ok(process);

        }

        [HttpPost("AddProcess")]
        public ActionResult AddProcess([FromBody] Process process)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _processService.AddProcess(process);
            if (success)
            {
                return Ok("Process was added");
            }
            return BadRequest("Failed to add process");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProcess(Guid id, [FromBody] Process process)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _processService.UpdateProcess(id, process);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update process");
        }

        [HttpDelete("{id}")]

        public ActionResult DeleteProcess(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _processService.DeleteProcess(id);
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
