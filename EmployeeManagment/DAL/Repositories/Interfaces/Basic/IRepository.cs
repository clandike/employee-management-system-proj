namespace DAL.Repositories.Interfaces.Basic
{
    public interface IRepository<T> : IReadableRepository<T>, IWritableRepository<T>
    {
    }
}
