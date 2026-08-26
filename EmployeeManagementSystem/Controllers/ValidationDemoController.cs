using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.DTOs;

namespace EmployeeManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValidationDemoController : ControllerBase
{
    [HttpPost]
    public IActionResult ValidateEmployee(ValidationDemoDto employee)
    {
        return Ok(employee);
    }
}