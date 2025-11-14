using DAL.Models;

namespace DAL.Repositories.Interfaces.Basic
{
    public interface ICreateReturnId<T>
    {
        Task<int?> CreateReturnIdAsync(EmployeeInfo entity);
    }
}
