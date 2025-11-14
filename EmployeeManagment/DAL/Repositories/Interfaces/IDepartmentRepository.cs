using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IDepartmentRepository :
        IGetAll<Department>,
        IGetById<Department>
    {
    }
}
