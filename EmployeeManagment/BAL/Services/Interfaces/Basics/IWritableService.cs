using DAL.Models;

namespace BAL.Services.Interfaces.Basics
{
    public interface IWritableService<T>
    {
        public Task SaveAsync(T model);

        public Task DeleteAsync(int id);
    }
}
