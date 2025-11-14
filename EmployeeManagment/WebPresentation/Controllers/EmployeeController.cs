using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Models;
using WebPresentation.Service.Interfaces;

[Route("Employees")]
public class EmployeeController : Controller
{
    private readonly IEmployeeAppService employeeAppService;

    public EmployeeController(IEmployeeAppService employeeAppService)
    {
        this.employeeAppService = employeeAppService;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 1, string search = "", string department = "", string sort = "name")
    {
        var filter = new EmployeeFilterDto
        {
            Page = page,
            PageSize = 8,
            Search = search,
            Department = department,
            Sort = sort
        };

        var model = await employeeAppService.GetListAsync(filter);
        var dics = await employeeAppService.GetListOfDepartmentsAsync();
        var departmentDict = dics.ToDictionary(d => d.Id, d => d.Name);

        ViewData["Search"] = search;
        ViewData["Department"] = department;
        ViewData["Sort"] = sort;
        ViewBag.Departments = departmentDict.Values
                .Distinct()
                .OrderBy(d => d)
                .ToList();

        return View(model);
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> DetailsAsync(int id)
    {
        var employee = await employeeAppService.GetByIdAsync(id);
        return employee is null ? NotFound() : View(employee);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> CreateAsync()
    {
        ViewBag.Departments = await employeeAppService.GetDepartmentsSelectListAsync();
        ViewBag.Positions = await employeeAppService.GetPositionsSelectListAsync();
        return View("Upsert", new EmployeeFullViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(EmployeeFullViewModel dto)
    {
        await employeeAppService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> EditAsync(int id)
    {
        var employee = await employeeAppService.GetByIdAsync(id);
        if (employee is null) return NotFound();

        var dto = new EmployeeFullViewModel
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            MiddleName = employee.MiddleName,
            Address = employee.Address,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email,
            BirthDate = employee.BirthDate,
            HireDate = employee.HireDate,
            Salary = employee.Salary,
            DepartmentId = employee.DepartmentId,
            PositionId = employee.PositionId
        };

        ViewBag.Departments = await employeeAppService.GetDepartmentsSelectListAsync(dto.DepartmentId);
        ViewBag.Positions = await employeeAppService.GetPositionsSelectListAsync(dto.PositionId);

        return View("Upsert", dto);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(int id, EmployeeFullViewModel dto)
    {
        if (id != dto.Id) return BadRequest();
        await employeeAppService.UpdateAsync(id, dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await employeeAppService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}