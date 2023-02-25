using Dapper;
using UsingDapperApi.Context;
using UsingDapperApi.Dto;
using UsingDapperApi.Entities;

namespace UsingDapperApi.Repository.Impl
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DapperContext _context;

        public CompanyRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Company> CreateCompanyAsync(CompanyForCreationDto company)
        {
            string sql = "INSERT INTO Companies(name, address, country) VALUES(@name, @address, @country)";

            // Transaction
            using (var conn = _context.CreateConnection())
            using (var tran = conn.BeginTransaction())
            {
                await Dapper.SqlMapper.ExecuteAsync(conn, sql, new { name = company.Name, address = company.Address, country = company.Country });

                var createCompany = new Company
                {
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country,
                };

                tran.Commit();
                return createCompany;
            }
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            string sql = "SELECT * FROM Companies";

            using (var conn = _context.CreateConnection())
            {
                var companies = await Dapper.SqlMapper.QueryAsync<Company>(conn, sql);
                return companies;
            }
        }

        public async Task<Company> GetCompanyAsync(int id)
        {
            string sql = "SELECT * FROM Companies WHERE id = @id";

            using (var conn = _context.CreateConnection())
            {
                var company = await Dapper.SqlMapper.QueryFirstOrDefaultAsync<Company>(conn, sql, new { id });
                return company;
            }
        }

        public async Task<Company> UpdateCompanyAsync(int id, CompanyForUpdateDto company)
        {
            string sql = @"
UPDATE 
    Companies 
SET 
    name = @name, address = @address, country = @country 
WHERE 
    id = @id";

            using (var conn = _context.CreateConnection())
            {
                await Dapper.SqlMapper.ExecuteAsync(conn, sql, new { name = company.Name, address = company.Address, country = company.Country, id = id });

                var updateCompany = new Company
                {
                    Name = company.Name,
                    Address = company.Address,
                    Country = company.Country,
                };

                return updateCompany;
            }
        }

        public async Task DeleteCompanyAsync(int id)
        {
            string sql = "DELETE FROM Companies WHERE id = @id";

            using (var conn = _context.CreateConnection())
            {
                await Dapper.SqlMapper.ExecuteAsync(conn, sql, new { id });
            }
        }

        public async Task<List<Company>> GetCompaniesEmployeesMultipleMappingAsync()
        {
            string sql = @"
SELECT 
    * 
FROM 
    Companies AS c 
JOIN 
    Employees AS e 
ON 
    c.Id = e.CompanyId";

            using (var conn = _context.CreateConnection())
            {
                var companyDict = new Dictionary<int, Company>();

                var companies = await conn.QueryAsync<Company, Employee, Company>(sql,
                    (company, employee) =>
                {
                    if (!companyDict.TryGetValue(company.Id, out var currentCompany))
                    {
                        currentCompany = company;
                        companyDict.Add(currentCompany.Id, currentCompany);
                    }

                    currentCompany.Employees.Add(employee);
                    return currentCompany;
                });

                return companies.Distinct().ToList();
            }
        }

        public async Task<Company> GetCompanyEmployeesMultipleResults(int id)
        {
            string sql = @"SELECT * FROM Companies WHERE id = @id;
                            SELECT * FROM Employees WHERE companyId = @id";

            using (var conn = _context.CreateConnection())
            using (var multi = await conn.QueryMultipleAsync(sql, new { id }))
            {
                var company = await multi.ReadSingleOrDefaultAsync<Company>();
                if (company != null)
                {
                    company.Employees = (await multi.ReadAsync<Employee>()).ToList();
                }

                return company;
            }
        }
    }
}
