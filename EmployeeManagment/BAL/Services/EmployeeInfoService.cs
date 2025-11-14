using AutoMapper;
using BAL.DTO;
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
            var entityEmployee = mapper.Map<EmployeeInfo>(employee);
            await employeeInfoRepository.UpdateAsync(entityEmployee);
        }

        public async Task<int?> CreateReturnIdAsync(EmployeeInfoDTO employee)
        {
            var entityEmployee = mapper.Map<EmployeeInfo>(employee);
            int? id = await employeeInfoRepository.CreateReturnIdAsync(entityEmployee);
            return id;
        }

        public async Task DeleteAsync(int id)
        {
            await employeeInfoRepository.DeleteAsync(id);
        }
    }
}
