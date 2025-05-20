using EmployeeManagementMVC.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeManagementMVC.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _connectionString;

        public EmployeeService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("SELECT * FROM Employees", conn);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    Id = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Department = reader.GetString(4),
                    HireDate = reader.GetDateTime(5),
                    Salary = reader.GetDecimal(6)
                });
            }

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("SELECT * FROM Employees WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Employee
                {
                    Id = reader.GetGuid(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    Email = reader.GetString(3),
                    Department = reader.GetString(4),
                    HireDate = reader.GetDateTime(5),
                    Salary = reader.GetDecimal(6)
                };
            }

            return null;
        }

        public async Task AddAsync(Employee employee)
        {
            employee.Id = Guid.NewGuid();

            using SqlConnection conn = new(_connectionString);
            string query = @"INSERT INTO Employees (Id, FirstName, LastName, Email, Department, HireDate, Salary)
                             VALUES (@Id, @FirstName, @LastName, @Email, @Department, @HireDate, @Salary)";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Id", employee.Id);
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);
            cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            using SqlConnection conn = new(_connectionString);
            string query = @"UPDATE Employees SET 
                             FirstName=@FirstName, LastName=@LastName, Email=@Email, 
                             Department=@Department, HireDate=@HireDate, Salary=@Salary
                             WHERE Id=@Id";
            using SqlCommand cmd = new(query, conn);
            cmd.Parameters.AddWithValue("@Id", employee.Id);
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);
            cmd.Parameters.AddWithValue("@HireDate", employee.HireDate);
            cmd.Parameters.AddWithValue("@Salary", employee.Salary);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("DELETE FROM Employees WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
