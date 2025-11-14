namespace BAL.Services.Interfaces.Basics
{
    public interface IGetAll<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();
    }
}
