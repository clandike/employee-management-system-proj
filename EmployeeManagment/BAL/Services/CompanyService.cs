using DAL.Repositories.Interfaces;

namespace BAL.Services
{
    public class CompanyService
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        public async Task<IEnumerable<DAL.Models.Company>> GetAllCompaniesAsync()
        {
            return await companyRepository.GetAllAsync();
        }
    }
}
