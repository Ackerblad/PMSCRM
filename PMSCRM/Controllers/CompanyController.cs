using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Authorize]
    [Route("[controller]")]
    //[ApiController]
    public class CompanyController : Controller
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetAll")]
        public ActionResult<List<Company>> GetAll()
        {
            var companies = _companyService.GetAll();
            if (companies == null || companies.Count == 0)
            {
                return NotFound("No companies found");
            }
            return Ok(companies);
        }

        [HttpGet("GetCompanyNames")]
        public async Task<IActionResult> GetCompanyNames(string term)
        {
            var companyNames = _companyService.GetCompanyNames(term); // Fetch matching names from DB
            return Ok(companyNames);
        }


        //[HttpPost("Add")]
        //public ActionResult Add([FromBody] Company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var success = _companyService.Add(company);
        //    if (success)
        //    {
        //        return Ok("Company was added");
        //    }
        //    return BadRequest("Failed to add company");
        //}

        [HttpPost]
        public ActionResult AddCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return View(company);
            }

            bool success = _companyService.Add(company);
            if (success)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to add company");
            return View(company);
        }

        //[HttpPut("{id}")]
        //public ActionResult Update(Guid guid, [FromBody] Company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _companyService.Update(guid, company);
        //    if (success)
        //    {
        //        return Ok("Company was updated");
        //    }
        //    return BadRequest("Failed to update company");
        //}

        [HttpGet("EditCompany/{id}")]
        public IActionResult EditCompany(Guid id)
        {
            var company = _companyService.GetById(id);
            if (company == null)
            {
                return NotFound("Company not found");
            }
            return View(company);
        }

        [HttpPost("EditCompany/{id}")]
        public IActionResult EditCompany(Guid id, Company updatedCompany)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedCompany);
            }

            bool success = _companyService.Update(id, updatedCompany);
            if (success)
            {
                return RedirectToAction("ViewCompanies");
            }

            ModelState.AddModelError(string.Empty, "Failed to update company.");
            return View("updatedCompany");
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete(Guid guid)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    bool success = _companyService.Delete(guid);
        //    if (success)
        //    {
        //        return Ok();
        //    }

        //    return BadRequest("Failed to delete company");
        //}

        // GET: Task/DeleteTask/{id}
        [HttpGet("DeleteCompany/{id}")]
        public IActionResult DeleteCompany(Guid id)
        {
            var company = _companyService.GetById(id);
            if (company == null)
            {
                return NotFound("Company not found");
            }

            return View(company);
        }

        [HttpPost("DeleteConfirmed/{id}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var company = _companyService.GetById(id);
            if (company == null)
            {
                return NotFound("Company not found.");
            }

            bool success = _companyService.Delete(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Company deleted successfully!";
                return RedirectToAction("ViewCompanies");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete company.");
            return View("DeleteCompany", company);
        }

        public IActionResult AddCompany()
        {
            return View();
        }

        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("ViewCompanies")]
        public IActionResult ViewCompanies()
        {
            var company = _companyService.GetAll();
            return View(company);
        }
        [HttpGet("EditCompany")]
        public IActionResult EditCompany()
        {
            return View();
        }
        [HttpGet("DeleteCompany")]
        public IActionResult DeleteCompany()
        {
            return View();
        }
    }
}
