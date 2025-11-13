namespace DAL.Repositories.Interfaces.Basic
{
    public interface IReadableRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);
    }
}
