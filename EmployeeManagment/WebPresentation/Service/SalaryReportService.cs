using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using WebPresentation.Models;
using WebPresentation.Service.Interfaces;

namespace WebPresentation.Service
{
    public class SalaryReportService : ISalaryReportService
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeInfoService employeeInfoService;
        private readonly IDepartmentService departmentService;
        private readonly IPositionService positionService;

        public SalaryReportService(
            IEmployeeService employeeService,
            IEmployeeInfoService employeeInfoService,
            IDepartmentService departmentService,
            IPositionService positionService)
        {
            this.employeeService = employeeService;
            this.employeeInfoService = employeeInfoService;
            this.departmentService = departmentService;
            this.positionService = positionService;
        }

        public async Task<SalaryReportViewModel> GenerateReportAsync(SalaryReportFilter filter)
        {
            var employees = await employeeService.GetAllAsync();
            var infos = await employeeInfoService.GetAllAsync();
            var positions = (await positionService.GetAllAsync()).ToDictionary(p => p.Id, p => p.Title);
            var departments = (await departmentService.GetAllAsync()).ToDictionary(d => d.Id, d => d.Name);

            var query = employees
                .GroupJoin(infos, e => e.Id, i => i.Id, (e, g) => new { e, g })
                .SelectMany(x => x.g.DefaultIfEmpty(), (e, info) => new { e.e, info })
                .Where(x => x.info != null)
                .Select(x => new SalaryReportItem
                {
                    Id = x.e.Id,
                    FullName = $"{x.info!.LastName} {x.info.FirstName} {x.info.MiddleName}".Trim(),
                    Position = positions.GetValueOrDefault(x.e.PositionId) ?? "—",
                    PositionId = x.e.PositionId,
                    Department = departments.GetValueOrDefault(x.e.DepartmentId) ?? "—",
                    DepartmentId = x.e.DepartmentId,
                    Salary = x.e.Salary
                });

            if (filter.DepartmentId.HasValue)
                query = query.Where(x => x.DepartmentId == filter.DepartmentId.Value);

            if (filter.PositionId.HasValue)
                query = query.Where(x => x.PositionId == filter.PositionId.Value);

            var items = query.ToList();

            return new SalaryReportViewModel
            {
                Items = items,
                TotalSalary = items.Sum(x => x.Salary),
                EmployeeCount = items.Count
            };
        }

        public byte[] ExportToTxt(SalaryReportViewModel report)
        {
            var sb = new StringBuilder();
            sb.AppendLine("═".PadRight(90, '═'));
            sb.AppendLine("\t\t\t\tЗАРПЛАТНА ВІДОМІСТЬ".PadLeft(60));
            sb.AppendLine($"Станом на: {DateTime.Now:dd.MM.yyyy HH:mm}".PadLeft(70));
            sb.AppendLine("═".PadRight(90, '═'));
            sb.AppendLine();

            if (report.Items.Any())
            {
                var first = report.Items.First();
                sb.AppendLine($"Відділ: {first.Department}");
                if (first.PositionId > 0)
                    sb.AppendLine($"Посада: {first.Position}");
                sb.AppendLine("─".PadRight(90, '─'));
                sb.AppendLine($"{"ID",-5} {"ПІБ",-35} {"Посада",-20} {"Оклад, грн",15}");
                sb.AppendLine("─".PadRight(90, '─'));

                foreach (var item in report.Items)
                {
                    sb.AppendLine($"{item.Id,-5} {item.FullName,-35} {item.Position,-20} {item.Salary,15:N2}");
                }

                sb.AppendLine("─".PadRight(90, '─'));
                sb.AppendLine($"Всього працівників: {report.EmployeeCount}".PadLeft(85));
                sb.AppendLine($"ЗАГАЛЬНА СУМА ОКЛАДІВ: {report.TotalSalary,48:N2} грн".PadLeft(90));
            }
            else
            {
                sb.AppendLine("Дані за вибраними фільтрами відсутні.".PadLeft(60));
            }

            sb.AppendLine();
            sb.AppendLine("═".PadRight(90, '═'));

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public Task<SelectList> GetDepartmentsSelectListAsync(int? selectedId = null)
            => CreateSelectListAsync(departmentService.GetAllAsync(), "Id", "Name", selectedId);

        public Task<SelectList> GetPositionsSelectListAsync(int? selectedId = null)
            => CreateSelectListAsync(positionService.GetAllAsync(), "Id", "Title", selectedId);

        private static async Task<SelectList> CreateSelectListAsync<T>(
            Task<IEnumerable<T>> source,
            string dataValueField,
            string dataTextField,
            int? selectedValue)
        {
            var items = await source;
            return new SelectList(items, dataValueField, dataTextField, selectedValue);
        }
    }
}
