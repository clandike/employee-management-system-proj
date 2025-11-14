using BAL.Models;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IEmployeeService : IReadableService<EmployeeDTO>, IWritableService<EmployeeDTO>
    {
    }
}
