using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IEmployeeService : 
        ISave<EmployeeDTO>,
        IGetById<EmployeeDTO>,
        IGetAll<EmployeeDTO>,
        IDelete
    {
    }
}
