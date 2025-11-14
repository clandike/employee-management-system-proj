using AutoMapper;
using BAL.Models;
using DAL.Models;
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

        public async Task SaveAsync(DepartmentDTO department)
        {
            var emp = await departmentRepository.GetByIdAsync(department.Id);
            var entity = mapper.Map<Department>(department);

            if (emp != null)
            {
                await departmentRepository.UpdateAsync(entity);
            }
            else
            {
                await departmentRepository.CreateAsync(entity);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await departmentRepository.DeleteAsync(id);
        }
    }
}
