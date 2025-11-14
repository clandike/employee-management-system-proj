namespace BAL.Services.Interfaces.Basics
{
    public interface IReadableService<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetByIdAsync(int id);
    }
}
