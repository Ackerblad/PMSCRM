using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSCRM.Models;
using PMSCRM.Services;

namespace PMSCRM.Controllers
{
    [Authorize]
    [Route("[controller]")]
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
            var companyNames = _companyService.GetCompanyNames(term); 
            return Ok(companyNames);
        }

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
