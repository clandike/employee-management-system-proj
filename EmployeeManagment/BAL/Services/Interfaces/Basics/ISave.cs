namespace BAL.Services.Interfaces.Basics
{
    public interface ISave<T>
    {
        public Task SaveAsync(T model);
    }
}
