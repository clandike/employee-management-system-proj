namespace BAL.Services.Interfaces.Basics
{
    public interface ICreateWithId<T>
    {
        Task<int?> CreateReturnIdAsync(T entity);
    }
}
