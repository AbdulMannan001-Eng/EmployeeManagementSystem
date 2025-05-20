using EmployeeManagementMVC.Models;

namespace EmployeeManagementMVC.Services
{
    public interface IEmployeeService
    {
        Task AddAsync(Employee employee);
        Task DeleteAsync(Guid id);
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task UpdateAsync(Employee employee);
    }
}