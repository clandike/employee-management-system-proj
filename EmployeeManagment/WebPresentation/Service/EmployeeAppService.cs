using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPresentation.Models;
using WebPresentation.Service.Interfaces;

namespace WebPresentation.Service
{
    public class EmployeeAppService : IEmployeeAppService
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeInfoService employeeInfoService;
        private readonly IDepartmentService departmentService;
        private readonly IPositionService positionService;

        public EmployeeAppService(IEmployeeService employeeService,
        IEmployeeInfoService employeeInfoService,
        IDepartmentService departmentService,
        IPositionService positionService,
        IMapper mapper)
        {
            this.employeeService = employeeService;
            this.employeeInfoService = employeeInfoService;
            this.departmentService = departmentService;
            this.positionService = positionService;
        }

        public async Task CreateAsync(EmployeeFullViewModel dto)
        {
            var employeeInfo = new EmployeeInfoDTO()
            {
                Id = dto.Id,
                Address = dto.Address,
                BirthDate = dto.BirthDate,
                Email = dto.Email,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
            };

            int? id = await employeeInfoService.CreateReturnIdAsync(employeeInfo);

            var empl = new EmployeeDTO()
            {
                Id = id ?? 0,
                DepartmentId = dto.DepartmentId,
                HireDate = dto.HireDate,
                PositionId = dto.PositionId,
                Salary = dto.Salary,
            };

            await employeeService.SaveAsync(empl);
        }

        public async Task DeleteAsync(int id)
        {
            await employeeService.DeleteAsync(id);
            await employeeInfoService.DeleteAsync(id);
        }

        public async Task<EmployeeFullViewModel?> GetByIdAsync(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);
            var employeesInfo = await employeeInfoService.GetByIdAsync(id);

            var position = await positionService.GetByIdAsync(employee.PositionId);
            var department = await departmentService.GetByIdAsync(employee.DepartmentId);

            EmployeeFullViewModel employeeFull = new EmployeeFullViewModel()
            {
                Id = employee.Id,
                PositionId = employee.PositionId,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                Address = employeesInfo.Address,
                BirthDate = employeesInfo.BirthDate,
                Email = employeesInfo.Email,
                FirstName = employeesInfo.FirstName,
                MiddleName = employeesInfo.MiddleName,
                LastName = employeesInfo.LastName,
                PhoneNumber = employeesInfo.PhoneNumber,
                Position = position.Title,
                Department = department.Name,
            };

            return employeeFull;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetListOfDepartmentsAsync()
        {
            return await departmentService.GetAllAsync();
        }

        public async Task<SelectList> GetDepartmentsSelectListAsync(int? selectedId = null)
        {
            var value = new SelectList(await departmentService.GetAllAsync(), "Id", "Name", selectedId);
            return value;
        }

        public async Task<PagedResult<EmployeePreViewModel>> GetListAsync(EmployeeFilterDto filter)
        {
            var employees = await employeeService.GetAllAsync();
            var employeeInfos = await employeeInfoService.GetAllAsync();
            var positions = await positionService.GetAllAsync();
            var departments = await departmentService.GetAllAsync();

            var positionDict = positions.ToDictionary(p => p.Id, p => p.Title);
            var departmentDict = departments.ToDictionary(d => d.Id, d => d.Name);

            var query = from e in employees
                        join info in employeeInfos on e.Id equals info.Id into infoGroup
                        from info in infoGroup.DefaultIfEmpty()
                        where info != null
                        select new EmployeePreViewModel
                        {
                            Id = e.Id,
                            FullName = string.Join(" ",
                                info.LastName, info.FirstName, info.MiddleName)
                                .Trim(),
                            Position = positionDict.GetValueOrDefault(e.PositionId) ?? "—",
                            Department = departmentDict.GetValueOrDefault(e.DepartmentId) ?? "—"
                        };

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(x => x.FullName.Contains(filter.Search, StringComparison.OrdinalIgnoreCase) ||
                                        x.Position.Contains(filter.Search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filter.Department))
            {
                query = query.Where(x => x.Department == filter.Department);
            }

            query = filter.Sort switch
            {
                "department" => query.OrderBy(x => x.Department).ThenBy(x => x.FullName),
                "position" => query.OrderBy(x => x.Position).ThenBy(x => x.FullName),
                _ => query.OrderBy(x => x.FullName)
            };

            var total = query.Count();
            var items = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var model = new PagedResult<EmployeePreViewModel>
            {
                Items = items,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = total
            };

            return model;
        }

        public async Task<SelectList> GetPositionsSelectListAsync(int? selectedId = 1)
        {
            var value = new SelectList(await positionService.GetAllAsync(), "Id", "Title", selectedId);
            return value;
        }

        public async Task UpdateAsync(int id, EmployeeFullViewModel employee)
        {
            var employeeInfo = new EmployeeInfoDTO()
            {
                Id = employee.Id,
                Address = employee.Address,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
            };

            var empl = new EmployeeDTO()
            {
                Id = employee.Id,
                DepartmentId = employee.DepartmentId,
                HireDate = employee.HireDate,
                PositionId = employee.PositionId,
                Salary = employee.Salary,
            };

            await employeeInfoService.SaveAsync(employeeInfo);
            await employeeService.SaveAsync(empl);
        }
    }
}
