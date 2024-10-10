using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;
using PMSCRM.Utilities;

namespace PMSCRM.Controllers
{
    [Route("[controller]")]
    public class TaskProcessAreaUserCustomerController : Controller
    {
        private readonly TaskProcessAreaUserCustomerService _taskProcessAreaUserCustomerService;
        private readonly CompanyDivider _companyDivider;

        public TaskProcessAreaUserCustomerController(TaskProcessAreaUserCustomerService taskProcessAreaUserCustomerService, CompanyDivider companyDivider)
        {
            _taskProcessAreaUserCustomerService = taskProcessAreaUserCustomerService;
            _companyDivider = companyDivider;
        }

        //[HttpGet("GetAll")]
        //public ActionResult<List<TaskProcessAreaUserCustomer>> GetAll()
        //{
        //    var companyId = _companyDivider.GetCompanyId();
        //    var tpauc = _taskProcessAreaUserCustomerService.GetAll(companyId);
        //    if (tpauc == null || !tpauc.Any())
        //    {
        //        return NotFound("Nothing found");
        //    }
        //    return Ok(tpauc);
        //}

        [HttpPost("Add")]
        public ActionResult Add([FromBody] TaskProcessAreaUserCustomer tpauc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tpauc.CompanyId = _companyDivider.GetCompanyId();

            var success = _taskProcessAreaUserCustomerService.Add(tpauc);
            if (success)
            {
                return Ok("It was added");
            }
            return BadRequest("Failed to add it");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, [FromBody] TaskProcessAreaUserCustomer tpauc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tpauc.CompanyId = _companyDivider.GetCompanyId();

            bool success = _taskProcessAreaUserCustomerService.Update(id, tpauc);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Failed to update");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _taskProcessAreaUserCustomerService.Delete(id);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
