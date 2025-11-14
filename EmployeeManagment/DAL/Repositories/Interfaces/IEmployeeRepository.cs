using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IEmployeeRepository:
        ICreate<Employee>,
        IUpdate<Employee>,
        IGetAll<Employee>,
        IGetById<Employee>,
        IDelete
    {
    }
}
