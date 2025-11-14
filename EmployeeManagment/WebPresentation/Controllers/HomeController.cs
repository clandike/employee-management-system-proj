using System.Diagnostics;
using BAL.Services;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Models;

namespace WebPresentation.Controllers
{
    public class HomeController : Controller
    {
        private const int COMPANY_ID = 1;
        private readonly ILogger<HomeController> _logger;

        private readonly CompanyService companyService;

        public HomeController(CompanyService companyService)
        {
            this.companyService = companyService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var companies = await companyService.GetAllCompaniesAsync();
            var company = companies.FirstOrDefault(x => x.Id == COMPANY_ID);
            return View(company);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
