namespace DAL.Repositories.Interfaces.Basic
{
    public interface IGetAll<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
