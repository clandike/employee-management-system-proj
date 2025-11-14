using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository positionRepository;
        private readonly IMapper mapper;

        public PositionService(IPositionRepository positionRepository, IMapper mapper)
        {
            this.positionRepository = positionRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PositionDTO>> GetAllAsync()
        {
            var entities = await positionRepository.GetAllAsync();
            IEnumerable<PositionDTO> positions = mapper.Map<IEnumerable<PositionDTO>>(entities);
            return positions;
        }

        public async Task<PositionDTO> GetByIdAsync(int id)
        {
            var entity = await positionRepository.GetByIdAsync(id);
            PositionDTO position = mapper.Map<PositionDTO>(entity);
            return position;
        }
    }
}
