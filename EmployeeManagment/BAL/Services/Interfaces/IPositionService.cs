using BAL.Models;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IPositionService : IReadableService<PositionDTO>, IWritableService<PositionDTO>
    {
    }
}
