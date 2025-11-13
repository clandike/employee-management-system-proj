using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeService(IEmployeeRepository companyRepository)
        {
            this.employeeRepository = companyRepository;
        }

        public async Task<IEnumerable<DAL.Models.Employee>> GetAllCompaniesAsync()
        {
            return await employeeRepository.GetAllAsync();
        }
    }
}
