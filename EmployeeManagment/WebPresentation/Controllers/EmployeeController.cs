using BAL.Models;
using BAL.Services;
using BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebPresentation.Models;

namespace WebPresentation.Controllers
{
    [Controller]
    [Route("/Employees")]
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

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string search = "", string department = "", string sort = "name")
        {
            const int pageSize = 12;

            var employees = await employeeService.GetAllAsync();
            var employeeInfos = await employeeInfoService.GetAllAsync();
            var positions = await positionService.GetAllAsync();
            var departments = await departmentService.GetAllAsync();

            var positionDict = positions.ToDictionary(p => p.Id, p => p.Title);
            var departmentDict = departments.ToDictionary(d => d.Id, d => d.Name);

            var query = from e in employees
                        join info in employeeInfos on e.Id equals info.Id into infoGroup
                        from info in infoGroup.DefaultIfEmpty()
                        where info != null
                        select new EmployeePreViewModel
                        {
                            Id = e.Id,
                            FullName = string.Join(" ",
                                info.LastName, info.FirstName, info.MiddleName)
                                .Trim(),
                            Position = positionDict.GetValueOrDefault(e.PositionId) ?? "—",
                            Department = departmentDict.GetValueOrDefault(e.DepartmentId) ?? "—"
                        };

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.FullName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                        x.Position.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(x => x.Department == department);
            }

            query = sort switch
            {
                "department" => query.OrderBy(x => x.Department).ThenBy(x => x.FullName),
                "position" => query.OrderBy(x => x.Position).ThenBy(x => x.FullName),
                _ => query.OrderBy(x => x.FullName)
            };

            var total = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = new PagedResult<EmployeePreViewModel>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };

            ViewData["Search"] = search;
            ViewData["Department"] = department;
            ViewData["Sort"] = sort;

            ViewBag.Departments = departmentDict.Values
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            return View(model);
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
                HireDate = employee.HireDate,
                Address = employeesInfo.Address,
                BirthDate = employeesInfo.BirthDate,
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
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = new SelectList(await departmentService.GetAllAsync(), "Id", "Name");
            ViewBag.Positions = new SelectList(await positionService.GetAllAsync(), "Id", "Title");
            return View("Upsert", new EmployeeFullViewModel());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(EmployeeFullViewModel employee)
        {
            var employeeInfo = new EmployeeInfoDTO()
            {
                Id = employee.Id,
                Address = employee.Address,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
            };

            int? id = await employeeInfoService.CreateReturnIdAsync(employeeInfo);

            var empl = new EmployeeDTO()
            {
                Id = id ?? 0,
                DepartmentId = employee.DepartmentId,
                HireDate = employee.HireDate,
                PositionId = employee.PositionId,
                Salary = employee.Salary,
            };

            await employeeService.SaveAsync(empl);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
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
                HireDate = employee.HireDate,
                Address = employeesInfo.Address,
                BirthDate = employeesInfo.BirthDate,
                Email = employeesInfo.Email,
                FirstName = employeesInfo.FirstName,
                MiddleName = employeesInfo.MiddleName,
                LastName = employeesInfo.LastName,
                PhoneNumber = employeesInfo.PhoneNumber,
                Position = position.Title,
                Department = department.Name,
            };

            if (employee == null)
                return NotFound();
            ViewBag.Departments = new SelectList(await departmentService.GetAllAsync(), "Id", "Name", employee.DepartmentId);
            ViewBag.Positions = new SelectList(await positionService.GetAllAsync(), "Id", "Title", employee.PositionId);


            return View("Upsert", employeeFull);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(EmployeeFullViewModel employee)
        {
            var employeeInfo = new EmployeeInfoDTO()
            {
                Id = employee.Id,
                Address = employee.Address,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
            };

            var empl = new EmployeeDTO()
            {
                Id = employee.Id,
                DepartmentId = employee.DepartmentId,
                HireDate = employee.HireDate,
                PositionId = employee.PositionId,
                Salary = employee.Salary,
            };

            await employeeInfoService.SaveAsync(employeeInfo);
            await employeeService.SaveAsync(empl);

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
