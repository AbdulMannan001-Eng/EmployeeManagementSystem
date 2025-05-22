
using EmployeeMgtSystem.Models;
using System.ComponentModel.DataAnnotations;
namespace EmployeeMgtSystem.ViewModel
{


    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [RegularExpression(@"^[A-Za-z]{2,}(?: [A-Za-z]{2,})*$", ErrorMessage = "Only letters allowed (2–50 characters).")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [RegularExpression(@"^[A-Za-z]{2,}(?: [A-Za-z]{2,})*$", ErrorMessage = "Only letters allowed (2–50 characters).")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        [RegularExpression(@"^[A-Za-z]{2,}(?: [A-Za-z]{2,})*$", ErrorMessage = "Only letters and spaces allowed.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Must be a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hire date is required.")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, 1000000, ErrorMessage = "Salary must be between $0 and $1,000,000.")]
        public decimal Salary { get; set; }
    }
}