using UsingDapperApi.Dto;
using UsingDapperApi.Entities;

namespace UsingDapperApi.Repository
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<Company>> GetCompaniesAsync();
        public Task<Company> GetCompanyAsync(int id);
        public Task<Company> CreateCompanyAsync(CompanyForCreationDto company);
        public Task<Company> UpdateCompanyAsync(int id, CompanyForUpdateDto company);
        public Task DeleteCompanyAsync(int id);


        public Task<Company> GetCompanyEmployeesMultipleResults(int id);
        public Task<List<Company>> GetCompaniesEmployeesMultipleMappingAsync();
    }
}
