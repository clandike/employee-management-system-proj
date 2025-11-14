using BAL.Models;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IEmployeeInfoService : IReadableService<EmployeeInfoDTO>, IWritableService<EmployeeInfoDTO>
    {
    }
}
