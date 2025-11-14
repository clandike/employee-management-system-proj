using DAL.Models;

namespace DAL.Repositories.Interfaces.Basic
{
    internal interface ICreateWithId<T>
    {
        Task<int?> CreateReturnIdAsync(EmployeeInfo entity);
    }
}
