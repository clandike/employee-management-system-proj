using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class CompanyService
    {
        private readonly IReadableCompanyRepository companyRepository;

        public CompanyService(IReadableCompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task<IEnumerable<DAL.Models.Company>> GetAllCompaniesAsync()
        {
            return await companyRepository.GetAllAsync();
        }
    }
}
