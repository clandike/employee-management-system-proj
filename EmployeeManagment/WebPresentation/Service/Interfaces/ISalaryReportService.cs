using Microsoft.AspNetCore.Mvc.Rendering;
using WebPresentation.Models;

namespace WebPresentation.Service.Interfaces
{
    public interface ISalaryReportService
    {
        Task<SalaryReportViewModel> GenerateReportAsync(SalaryReportFilter filter);

        byte[] ExportToTxt(SalaryReportViewModel report);

        Task<SelectList> GetDepartmentsSelectListAsync(int? selectedId = null);

        Task<SelectList> GetPositionsSelectListAsync(int? selectedId = null);
    }
}
