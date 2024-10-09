using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
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

        //[HttpPost("Add")]
        //public ActionResult Add([FromBody] Process process)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _processService.Add(process);
        //    if (success)
        //    {
        //        return Ok("Process was added");
        //    }
        //    return BadRequest("Failed to add process");
        //}
        [HttpPost("AddProcess")]
        public IActionResult AddProcess(Process process)
        {
            if (!ModelState.IsValid)
            {
                return View(process);
            }

            bool success = _processService.Add(process);
            if (success)
            {
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to add process.");
            return View(process);
        }

        //[HttpPut("{id}")]
        //public ActionResult Update(Guid guid, [FromBody] Process process)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _processService.Update(guid, process);
        //    if (success)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest("Failed to update process");
        //}
        // GET: /Process/EditProcess/{id}
        [HttpGet("EditProcess/{id}")]
        public IActionResult EditProcess(Guid id)
        {
            var process = _processService.GetById(id);
            if (process == null)
            {
                return NotFound("Process not found.");
            }
            return View(process);
        }

        // POST: /Process/EditProcess/{id}
        [HttpPost("EditProcess/{id}")]
        public IActionResult EditProcess(Guid id, Process updatedProcess)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedProcess);
            }

            bool success = _processService.Update(id, updatedProcess);
            if (success)
            {
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to update process.");
            return View(updatedProcess);
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete(Guid guid)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _processService.Delete(guid);
        //    if (success)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest("Failed to delete process");
        //}

        // GET: /Process/DeleteProcess/{id}
        [HttpGet("DeleteProcess/{id}")]
        public IActionResult DeleteProcess(Guid id)
        {
            var process = _processService.GetById(id);
            if (process == null)
            {
                return NotFound("Process not found.");
            }

            return View(process);
        }

        // POST: /Process/DeleteConfirmed/{id}
        [HttpPost("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var process = _processService.GetById(id);
            if (process == null)
            {
                return NotFound("Process not found.");
            }

            bool success = _processService.Delete(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Process deleted successfully!";
                return RedirectToAction("ViewProcesses");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete process.");
            return View("DeleteProcess", process);
        }

        [HttpGet("ViewProcesses")]
        public IActionResult ViewProcesses()
        {
            var processes = _processService.GetAll();
            if (processes == null || processes.Count == 0)
            {
                TempData["InfoMessage"] = "No processes available.";
            }
            return View(processes);
        }

        [HttpGet("AddProcess")]
        public IActionResult AddProcess()
        {
            return View();
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("EditProcess")]
        public IActionResult EditProcess()
        {
            return View();
        }

        [HttpGet("DeleteProcess")]
        public IActionResult DeleteProcess()
        {
            return View();
        }


    }
}
