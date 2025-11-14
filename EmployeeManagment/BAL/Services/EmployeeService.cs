using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IMapper mapper;

        public EmployeeService(IEmployeeRepository companyRepository, IMapper mapper)
        {
            this.employeeRepository = companyRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
            var entityEmployees = await employeeRepository.GetAllAsync();
            IEnumerable<EmployeeDTO> employees = mapper.Map<IEnumerable<EmployeeDTO>>(entityEmployees);
            return employees;
        }

        public async Task<EmployeeDTO> GetByIdAsync(int id)
        {
            var entityEmployee = await employeeRepository.GetByIdAsync(id);
            EmployeeDTO employee = mapper.Map<EmployeeDTO>(entityEmployee);
            return employee;
        }

        public async Task SaveAsync(EmployeeDTO employee)
        {
            var emp = await employeeRepository.GetByIdAsync(employee.Id);
            var entityEmployee = mapper.Map<Employee>(employee);

            if (emp != null)
            {
                await employeeRepository.UpdateAsync(entityEmployee);
            }
            else
            {
                await employeeRepository.CreateAsync(entityEmployee);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await employeeRepository.DeleteAsync(id);
        }
    }
}
