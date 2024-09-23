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

        [HttpGet("GetCompanies")]
        public ActionResult<List<Company>> GetCompanies()
        {
            var companies = _companyService.GetCompanies();
            if (companies == null || !companies.Any())
            {
                return NotFound("No companies found");
            }
            return Ok(companies);
        }

        [HttpPost("AddCompany")]
        public ActionResult AddCompany([FromBody] Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _companyService.AddCompany(company);
            if (success)
            {
                return Ok("Company was added");
            }
            return BadRequest("Failed to add company");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCompany(Guid id, [FromBody] Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _companyService.UpdateCompany(id, company);
            if (success)
            {
                return Ok("Company was updated");
            }
            return BadRequest("Failed to update company");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCompany(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = _companyService.DeleteCompany(id);
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
