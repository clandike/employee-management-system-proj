using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IEmployeeInfoService : 
        ICreateWithId<EmployeeInfoDTO>,
        ISave<EmployeeInfoDTO>,
        IGetById<EmployeeInfoDTO>,
        IGetAll<EmployeeInfoDTO>,
        IDelete
    {
    }
}
