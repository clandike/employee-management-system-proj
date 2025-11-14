using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;
        private readonly IMapper mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            this.departmentRepository = departmentRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            var entities = await departmentRepository.GetAllAsync();
            IEnumerable<DepartmentDTO> departments = mapper.Map<IEnumerable<DepartmentDTO>>(entities);
            return departments;
        }

        public async Task<DepartmentDTO> GetByIdAsync(int id)
        {
            var entity = await departmentRepository.GetByIdAsync(id);
            DepartmentDTO department = mapper.Map<DepartmentDTO>(entity);
            return department;
        }
    }
}
