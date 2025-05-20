using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementMVC.Models
{
    public class Employee
    {
        public Guid Id { get; set; }

        //[Required]
        public string FirstName { get; set; } 
        //[Required]
        public string LastName { get; set; }

        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required]
        public string Department { get; set; } 
        //[Required]
        public DateTime HireDate { get; set; }

        //[Required]
        public decimal Salary { get; set; }
    }
}
