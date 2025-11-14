using BAL.Services;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using WebPresentation.Models;

namespace WebPresentation.Controllers
{
    public class SalaryReportController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeInfoService employeeInfoService;
        private readonly IDepartmentService departmentService;
        private readonly IPositionService positionService;

        public SalaryReportController(
            IEmployeeService employeeService,
            IDepartmentService departmentService,
            IPositionService positionService,
            IEmployeeInfoService employeeIfnoService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.positionService = positionService;
            this.employeeInfoService = employeeIfnoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await PopulateDropdowns();
            return View(new SalaryReportViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(SalaryReportViewModel model)
        {
            await PopulateDropdowns(model.DepartmentId, model.PositionId);
            await GenerateReport(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ExportToTxt(SalaryReportViewModel model)
        {
            await GenerateReport(model); // заповнюємо дані

            var sb = new StringBuilder();
            sb.AppendLine("═".PadRight(80, '═'));
            sb.AppendLine("           ЗАРПЛАТНА ВІДОМІСТЬ");
            sb.AppendLine($"           Станом на: {DateTime.Now:dd.MM.yyyy HH:mm}");
            sb.AppendLine("═".PadRight(80, '═'));
            sb.AppendLine();

            if (model.Items.Any())
            {
                sb.AppendLine($"Відділ: {model.Items.First().Department}");
                if (model.PositionId.HasValue)
                    sb.AppendLine($"Посада: {model.Items.First().Position}");
                sb.AppendLine($"Період: {(model.DateFrom?.ToString("dd.MM.yyyy") ?? "-")} — {(model.DateTo?.ToString("dd.MM.yyyy") ?? "-")}");
                sb.AppendLine("─".PadRight(80, '─'));

                foreach (var item in model.Items)
                {
                    sb.AppendLine($"{item.Id,-5} {item.FullName,-30} {item.Position,-20} {item.Salary,12:N2} грн");
                }

                sb.AppendLine("─".PadRight(80, '─'));
                sb.AppendLine($"Всього працівників: {model.EmployeeCount}");
                sb.AppendLine($"ЗАГАЛЬНА СУМА ОКЛАДІВ: {model.TotalSalary,45:N2} грн");
            }
            else
            {
                sb.AppendLine("Дані відсутні за вибраними фільтрами.");
            }

            var fileName = $"SalaryReport_{DateTime.Now:yyyyMMdd_HHmm}.txt";
            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            return File(bytes, "text/plain; charset=utf-8", fileName);
        }

        private async Task GenerateReport(SalaryReportViewModel model)
        {

            var employees = await employeeService.GetAllAsync();
            var employeeInfos = await employeeInfoService.GetAllAsync();
            var positions = await positionService.GetAllAsync();
            var departments = await departmentService.GetAllAsync();

            var positionDict = positions.ToDictionary(p => p.Id, p => p.Title);
            var departmentDict = departments.ToDictionary(d => d.Id, d => d.Name);

            var result = from e in employees
                         join info in employeeInfos on e.Id equals info.Id into infoGroup
                         from info in infoGroup.DefaultIfEmpty()
                         where info != null
                         select new SalaryReportItem
                         {
                             Id = e.Id,
                             FullName = string.Join(" ",
                                 info.LastName, info.FirstName, info.MiddleName)
                                 .Trim(),
                             Position = positionDict.GetValueOrDefault(e.PositionId) ?? "—",
                             PositionId = e.PositionId,
                             Department = departmentDict.GetValueOrDefault(e.DepartmentId) ?? "—",
                             DepartmentId = e.DepartmentId,
                             Salary = e.Salary,
                         };

            if (model.DepartmentId.HasValue)
                result = result.Where(e => e.DepartmentId == model.DepartmentId);

            if (model.PositionId.HasValue)
                result = result.Where(e => e.PositionId == model.PositionId);

            model.Items = result;
            model.TotalSalary = result.Sum(x => x.Salary);
            model.EmployeeCount = result.Count();
        }

        private async Task PopulateDropdowns(int? selectedDept = null, int? selectedPos = null)
        {
            ViewBag.Departments = new SelectList(
                await departmentService.GetAllAsync(),
                "Id", "Name", selectedDept);

            ViewBag.Positions = new SelectList(
                await positionService.GetAllAsync(),
                "Id", "Title", selectedPos);
        }
    }
}
