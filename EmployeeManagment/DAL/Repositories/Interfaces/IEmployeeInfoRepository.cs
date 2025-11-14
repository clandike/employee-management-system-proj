using DAL.Models;
using DAL.Repositories.Interfaces.Basic;

namespace DAL.Repositories.Interfaces
{
    public interface IEmployeeInfoRepository : ICreateReturnId<EmployeeInfo>,
        IUpdate<EmployeeInfo>,
        IGetAll<EmployeeInfo>,
        IGetById<EmployeeInfo>,
        IDelete
    {
    }
}
