namespace DAL.Repositories.Interfaces.Basic
{
    public interface IWritableRepository<T>
    {
        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
