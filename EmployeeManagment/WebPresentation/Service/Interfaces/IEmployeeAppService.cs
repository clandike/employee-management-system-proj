using BAL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPresentation.Models;

namespace WebPresentation.Service.Interfaces
{
    public interface IEmployeeAppService
    {
        Task<PagedResult<EmployeePreViewModel>> GetListAsync(EmployeeFilterDto filter);
        Task<EmployeeFullViewModel?> GetByIdAsync(int id);
        Task CreateAsync(EmployeeFullViewModel dto);
        Task UpdateAsync(int id, EmployeeFullViewModel dto);
        Task DeleteAsync(int id);


        Task<IEnumerable<DepartmentDTO>> GetListOfDepartmentsAsync();
        Task<SelectList> GetDepartmentsSelectListAsync(int? selectedId = null);
        Task<SelectList> GetPositionsSelectListAsync(int? selectedId = null);
    }
}
