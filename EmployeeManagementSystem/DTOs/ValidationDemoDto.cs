using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs;

public class ValidationDemoDto
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    public string Department { get; set; } = string.Empty;

    [Range(1, 1000000, ErrorMessage = "Salary must be between 1 and 1000000.")]
    public decimal Salary { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
}