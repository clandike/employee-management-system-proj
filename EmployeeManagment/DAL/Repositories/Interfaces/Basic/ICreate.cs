namespace DAL.Repositories.Interfaces.Basic
{
    public interface ICreate<T>
    {
        Task CreateAsync(T entity);
    }
}
