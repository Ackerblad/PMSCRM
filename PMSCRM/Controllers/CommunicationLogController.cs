using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class CommunicationLogController : Controller
    {
        CommunicationLogService _communicationLogService;

        public CommunicationLogController(CommunicationLogService communicationLogService)
        {
            _communicationLogService = communicationLogService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<CommunicationLog>> GetAll()
        {
            var comLog = _communicationLogService.GetAll();
            if (comLog == null || !comLog.Any())
            {
                return NotFound("Nothing found");
            }
            return Ok(comLog);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] CommunicationLog comLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _communicationLogService.Add(comLog);
            if (success)
            {
                return Ok("Communication was added");
            }
            return BadRequest("Failed to add communication");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] CommunicationLog comLog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _communicationLogService.Update(guid, comLog);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update area");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
