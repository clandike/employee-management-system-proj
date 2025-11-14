namespace BAL.Services.Interfaces.Basics
{
    public interface IGetById<T>
    {
        public Task<T> GetByIdAsync(int id);
    }
}
