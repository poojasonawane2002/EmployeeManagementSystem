using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EmployeeManagementSystem.Interfaces;


namespace EmployeeManagementSystem.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public HealthController(
                                EmployeeDbContext context,
                                IEmployeeRepository employeeRepository,
                                IMapper mapper)
        {
            _context = context;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var employees = _context.Employees.ToList();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee);
        }

        [HttpGet("department/{department}")]
        public IActionResult GetByDepartment(string department)
        {
            var employees = _context.Employees
                                    .Where(e => e.Department == department)
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("first/{department}")]
        public IActionResult GetFirstEmployee(string department)
        {
            var employee = _context.Employees
                                   .FirstOrDefault(e => e.Department == department);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee);
        }

        [HttpGet("salary/ascending")]
        public IActionResult GetEmployeesBySalaryAscending()
        {
            var employees = _context.Employees
                                    .OrderBy(e => e.Salary)
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("salary/descending")]
        public IActionResult GetEmployeesBySalaryDescending()
        {
            var employees = _context.Employees
                                    .OrderByDescending(e => e.Salary)
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("count")]
        public IActionResult GetEmployeeCount()
        {
            var count = _context.Employees.Count();

            return Ok(count);
        }

        [HttpGet("exists/{department}")]
        public IActionResult CheckDepartmentExists(string department)
        {
            var exists = _context.Employees
                                 .Any(e => e.Department == department);

            return Ok(exists);
        }

        [HttpGet("basic")]
        public IActionResult GetBasicDetails()
        {
            var employees = _context.Employees
                                    .Select(e => new
                                    {
                                        e.Name,
                                        e.Email
                                    })
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("top/{count}")]
        public IActionResult GetTopEmployees(int count)
        {
            var employees = _context.Employees
                                    .Take(count)
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("skip/{count}")]
        public IActionResult SkipEmployees(int count)
        {
            var employees = _context.Employees
                                    .Skip(count)
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("pagination")]
        public IActionResult GetEmployeesWithPagination(int pageNumber = 1, int pageSize = 2)
        {
            var employees = _context.Employees
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("repository")]
        public async Task<IActionResult> GetFromRepository()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeeDtos = _mapper.Map<List<EmployeeResponseDto>>(employees);

            return Ok(employeeDtos);
        }

        [HttpGet("repository/{id}")]
        public async Task<IActionResult> GetByIdFromRepository(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<EmployeeResponseDto>(employee);

            return Ok(dto);
        }

        [HttpGet("itemployees")]
        public IActionResult GetITEmployees()
        {
            var employees = _context.Employees
                                    .Where(x => x.Department == "IT" && x.Salary > 50000)
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("search")]
        public IActionResult SearchEmployee(string name)
        {
            var employees = _context.Employees
                                    .Where(x => x.Name.Contains(name))
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("startswith")]
        public IActionResult StartsWithSearch(string name)
        {
            var employees = _context.Employees
                                    .Where(x => x.Name.StartsWith(name))
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("endswith")]
        public IActionResult EndsWithSearch(string email)
        {
            var employees = _context.Employees
                                    .Where(x => x.Email.EndsWith(email))
                                    .ToList();

            return Ok(employees);
        }

        [HttpGet("searchfilter")]
        public IActionResult SearchFilter(string? name, string? department)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(x => x.Department == department);
            }

            return Ok(query.ToList());
        }

        [HttpGet("salaryfilter")]
        public IActionResult SalaryFilter(decimal minSalary, decimal maxSalary)
        {
            var employees = _context.Employees
                .Where(x => x.Salary >= minSalary && x.Salary <= maxSalary)
                .ToList();

            return Ok(employees);
        }

        [HttpGet("employees")]
        public IActionResult GetEmployees(
    string? name,
    string? department,
    decimal? minSalary,
    decimal? maxSalary)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(department))
            {
                query = query.Where(x => x.Department == department);
            }

            if (minSalary.HasValue)
            {
                query = query.Where(x => x.Salary >= minSalary.Value);
            }

            if (maxSalary.HasValue)
            {
                query = query.Where(x => x.Salary <= maxSalary.Value);
            }

            return Ok(query.ToList());
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            employee.Name = "Welcome " + employee.Name;
            employee.Salary = employee.Salary + 10000;

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return Ok(employee);

           
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Employee updatedEmployee)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            employee.Name = updatedEmployee.Name;
            employee.Department = updatedEmployee.Department;
            employee.Salary = updatedEmployee.Salary;
            employee.Email = updatedEmployee.Email;

            _context.SaveChanges();

            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.Find(id);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return Ok("Employee deleted successfully");
        }

        [HttpPost("async")]
        public async Task<IActionResult> CreateAsync(Employee employee)
        {
            employee.Name = "Welcome " + employee.Name;
            employee.Salary = employee.Salary + 10000;

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpGet("async/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound("Employee Not Found");
            }

            return Ok(employee);
        }

        [HttpPut("async/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound("Employee Not Found");
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Department = employee.Department;
            existingEmployee.Salary = employee.Salary;
            existingEmployee.Email = employee.Email;

            await _context.SaveChangesAsync();

            return Ok(existingEmployee);
        }

        [HttpDelete("async/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound("Employee Not Found");
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return Ok("Employee Deleted Successfully");
        }

        // KAN-8: Create Employee API

        [HttpPost("createdto")]
        public async Task<IActionResult> CreateEmployeeDto(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Department = dto.Department,
                Salary = dto.Salary,
                Email = dto.Email
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByIdAsync), new { id = employee.Id }, employee);
        }

        [HttpPut("updatedto/{id}")]
        public async Task<IActionResult> UpdateEmployeeDto(int id, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = dto.Name;
            employee.Department = dto.Department;
            employee.Salary = dto.Salary;
            employee.Email = dto.Email;

            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpGet("responsedto")]
        public async Task<IActionResult> GetEmployeeResponse()
        {
            var employees = await _context.Employees.ToListAsync();

            var result = employees.Select(x => new EmployeeResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Department = x.Department,
                Email = x.Email
            }).ToList();

            return Ok(result);
        }

        [HttpPost("createautomapper")]
        public async Task<IActionResult> CreateAutoMapper(CreateEmployeeDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

    }
    
}






