using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsingDapperApi.Dto;
using UsingDapperApi.Repository;

namespace UsingDapperApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompanyController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            try
            {
                var companies = await _companyRepo.GetCompaniesAsync();
                return Ok(companies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyAsync(id);

                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(CompanyForCreationDto company)
        {
            try
            {
                var createCompany = await _companyRepo.CreateCompanyAsync(company);
                return Ok(createCompany);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyForUpdateDto company)
        {
            try
            {
                var dbCompany = await GetCompany(id);

                if (dbCompany == null)
                {
                    return NotFound();
                }

                var updateCompany = await _companyRepo.UpdateCompanyAsync(id, company);
                return Ok(updateCompany);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                var dbCompany = await GetCompany(id);

                if (dbCompany == null)
                {
                    return NotFound();  
                }

                await _companyRepo.DeleteCompanyAsync(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("MultipleMapping")]
        public async Task<IActionResult> GetCompaniesEmployeesMultipleMapping()
        {
            try
            {
                var companies = await _companyRepo.GetCompaniesEmployeesMultipleMappingAsync();
                return Ok(companies);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}/MultipleResult")]
        public async Task<IActionResult> GetCompanyEmployeesMultipleResult(int id)
        {
            try
            {
                var company = await _companyRepo.GetCompanyEmployeesMultipleResults(id);
                if (company == null)
                {
                    return NotFound();
                }

                return Ok(company); 
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
