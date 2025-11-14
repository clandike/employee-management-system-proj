using BAL.Models;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services
{
    public interface IDepartmentService : IReadableService<DepartmentDTO>, IWritableService<DepartmentDTO>
    {
    }
}
