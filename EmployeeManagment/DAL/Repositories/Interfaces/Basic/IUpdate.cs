namespace DAL.Repositories.Interfaces.Basic
{
    public interface IUpdate<T>
    {
        Task UpdateAsync(T entity);
    }
}
