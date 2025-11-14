using BAL.DTO;
using BAL.Services.Interfaces.Basics;

namespace BAL.Services.Interfaces
{
    public interface IPositionService :
        IGetById<PositionDTO>,
        IGetAll<PositionDTO>
    {
    }
}
