using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class EmployeeInfoService : IEmployeeInfoService
    {
        private readonly IEmployeeInfoRepository employeeInfoRepository;
        private readonly IMapper mapper;

        public EmployeeInfoService(IEmployeeInfoRepository employeeInfoRepository, IMapper mapper)
        {
            this.employeeInfoRepository = employeeInfoRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeInfoDTO>> GetAllAsync()
        {
            var entityEmployeеInfos = await employeeInfoRepository.GetAllAsync();
            IEnumerable<EmployeeInfoDTO> employeeInfos = mapper.Map<IEnumerable<EmployeeInfoDTO>>(entityEmployeеInfos);
            return employeeInfos;
        }

        public async Task<EmployeeInfoDTO> GetByIdAsync(int id)
        {
            var entityEmployeeInfo = await employeeInfoRepository.GetByIdAsync(id);
            EmployeeInfoDTO employeeInfo = mapper.Map<EmployeeInfoDTO>(entityEmployeeInfo);
            return employeeInfo;
        }

        public async Task SaveAsync(EmployeeInfoDTO employee)
        {
            var emp = await employeeInfoRepository.GetByIdAsync(employee.Id);
            var entityEmployee = mapper.Map<EmployeeInfo>(employee);

            if (emp != null)
            {
                await employeeInfoRepository.UpdateAsync(entityEmployee);
            }
            else
            {
                await employeeInfoRepository.CreateAsync(entityEmployee);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await employeeInfoRepository.DeleteAsync(id);
        }
    }
}
