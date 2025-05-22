
using System.ComponentModel.DataAnnotations;

namespace EmployeeMgtSystem.Models
{
    public class EmployeeHireDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime hireDate)
            {
                return hireDate <= DateTime.Today;
            }
            return false;
        }
    }
}