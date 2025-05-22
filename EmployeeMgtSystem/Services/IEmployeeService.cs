using EmployeeManagementMVC.Models;
using EmployeeMgtSystem.Models;

namespace EmployeeManagementModels.Services
{
    public interface IEmployeeService
    {
        Task<bool> AddAsync(Employee employee);
        Task DeleteAsync(Guid id);
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task UpdateAsync(Employee employee);
    }
}