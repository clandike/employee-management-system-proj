using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IDepartmentService : 
        IGetById<DepartmentDTO>,
        IGetAll<DepartmentDTO>
    {
    }
}
