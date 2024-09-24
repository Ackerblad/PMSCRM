using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Company>> GetAll()
        {
            var companies = _companyService.GetAll();
            if (companies == null || !companies.Any())
            {
                return NotFound("No companies found");
            }
            return Ok(companies);
        }

        [HttpPost("Add")]
        public ActionResult Add([FromBody] Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _companyService.Add(company);
            if (success)
            {
                return Ok("Company was added");
            }
            return BadRequest("Failed to add company");
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid guid, [FromBody] Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _companyService.Update(guid, company);
            if (success)
            {
                return Ok("Company was updated");
            }
            return BadRequest("Failed to update company");
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid guid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _companyService.Delete(guid);
            if (success)
            {
                return Ok();
            }

            return BadRequest("Failed to delete company");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
