namespace DAL.Repositories.Interfaces.Basic
{
    public interface IGetById<T>
    {
        Task<T> GetByIdAsync(int id);
    }
}
