using Microsoft.AspNetCore.Mvc;
using WebPresentation.Models;
using WebPresentation.Service.Interfaces;

namespace WebPresentation.Controllers
{
    [Route("SalaryReport")]
    public class SalaryReportController : Controller
    {
        private readonly ISalaryReportService reportService;

        public SalaryReportController(ISalaryReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            await PopulateDropdownsAsync();
            return View(new SalaryReportViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(SalaryReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(model.DepartmentId, model.PositionId);
                return View(model);
            }

            var filter = new SalaryReportFilter
            {
                DepartmentId = model.DepartmentId,
                PositionId = model.PositionId,
            };

            var report = await reportService.GenerateReportAsync(filter);

            model.Items = report.Items;
            model.TotalSalary = report.TotalSalary;
            model.EmployeeCount = report.EmployeeCount;

            await PopulateDropdownsAsync(model.DepartmentId, model.PositionId);
            return View(model);
        }

        [HttpPost("Export")]
        public async Task<IActionResult> ExportToTxtAsync(SalaryReportViewModel model)
        {
            var filter = new SalaryReportFilter
            {
                DepartmentId = model.DepartmentId,
                PositionId = model.PositionId
            };

            var report = await reportService.GenerateReportAsync(filter);
            var fileBytes = reportService.ExportToTxt(report);

            var fileName = $"SalaryReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            return File(fileBytes, "text/plain; charset=utf-8", fileName);
        }

        private async Task PopulateDropdownsAsync(int? dept = null, int? pos = null)
        {
            ViewBag.Departments = await reportService.GetDepartmentsSelectListAsync(dept);
            ViewBag.Positions = await reportService.GetPositionsSelectListAsync(pos);
        }
    }
}