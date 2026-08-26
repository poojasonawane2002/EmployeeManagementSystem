using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementSystem.DTOs;
public class CreateEmployeeDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Department { get; set; } = string.Empty;

    [Range(1, 1000000)]
    public decimal Salary { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}