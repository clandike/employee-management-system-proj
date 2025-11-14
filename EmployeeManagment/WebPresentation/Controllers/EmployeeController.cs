using BAL.Models;
using BAL.Services;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebPresentation.Models;

namespace WebPresentation.Controllers
{
    [Controller]
    [Route("/Employee")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeInfoService employeeInfoService;
        private readonly IDepartmentService departmentService;
        private readonly IPositionService positionService;

        public EmployeeController(IEmployeeService employeeService, IEmployeeInfoService employeeInfoService, IDepartmentService departmentService, IPositionService positionService)
        {
            this.employeeService = employeeService;
            this.employeeInfoService = employeeInfoService;
            this.departmentService = departmentService;
            this.positionService = positionService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> IndexAsync()
        {
            List<EmployeePreViewModel> models = new List<EmployeePreViewModel>();


            var employees = await employeeService.GetAllAsync();
            var employeesInfo = await employeeInfoService.GetAllAsync();


            var neeew = employees.Union(employees);

            foreach (var employee in employees)
            {
                models.Add(new EmployeePreViewModel()
                {
                    Id = employee.Id,
                    FullName = employee.Id.ToString(),
                    Position = employee.PositionId.ToString(),
                    Department = employee.DepartmentId.ToString(),
                });
            }

            return View(models);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);
            var employeesInfo = await employeeInfoService.GetByIdAsync(id);

            if (employee == null || employeesInfo == null)
                return NotFound();

            var position = await positionService.GetByIdAsync(employee.PositionId);
            var department = await departmentService.GetByIdAsync(employee.DepartmentId);

            EmployeeFullViewModel employeeFull = new EmployeeFullViewModel()
            {
                Id = employee.Id,
                PositionId = employee.PositionId,
                DepartmentId = employee.DepartmentId,
                Salary = employee.Salary,
                HireDate = employee.HireDate.ToString("d"),
                Address = employeesInfo.Address,
                BirthDate = employeesInfo.BirthDate.ToString("d"),
                Email = employeesInfo.Email,
                FirstName = employeesInfo.FirstName,
                MiddleName = employeesInfo.MiddleName,
                LastName = employeesInfo.LastName,
                PhoneNumber = employeesInfo.PhoneNumber,
                Position = position.Title,
                Department = department.Name,
            };

            return View(employeeFull);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View("Upsert", new EmployeeDTO());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
                return View("Upsert", employee);

            await employeeService.SaveAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View("Upsert", employee);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(EmployeeDTO employee)
        {
            if (!ModelState.IsValid)
                return View("Upsert", employee);

            await employeeService.SaveAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await employeeService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
