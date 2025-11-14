using AutoMapper;
using BAL.DTO;
using BAL.Services.Interfaces;
using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository companyRepository;
        private readonly IMapper mapper;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
        }

        public async Task<CompanyDTO> GetByIdAsync(int id)
        {
            var entity = await companyRepository.GetByIdAsync(id);
            var result = mapper.Map<CompanyDTO>(entity);
            return result;
        }
    }
}
