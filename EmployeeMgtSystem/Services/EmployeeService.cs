using System.Data;
using EmployeeManagementModels.Services;
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

        public async Task<bool> AddAsync(Employee employee)
        {
            employee.Id = Guid.NewGuid();

            using SqlConnection conn = new(_connectionString);
            await conn.OpenAsync();

            // Step 1: Check if email exists
            string checkQuery = "SELECT COUNT(1) FROM Employees WHERE Email = @Email";
            using SqlCommand checkCmd = new(checkQuery, conn);
            checkCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = employee.Email;

            int count = (int)await checkCmd.ExecuteScalarAsync();
            if (count > 0)
            {
                return false;
            }

            string insertQuery = @"INSERT INTO Employees 
                           (Id, FirstName, LastName, Email, Department, HireDate, Salary)
                           VALUES 
                           (@Id, @FirstName, @LastName, @Email, @Department, @HireDate, @Salary)";

            using SqlCommand insertCmd = new(insertQuery, conn);
            insertCmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = employee.Id;
            insertCmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = employee.FirstName;
            insertCmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = employee.LastName;
            insertCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = employee.Email;
            insertCmd.Parameters.Add("@Department", SqlDbType.NVarChar, 50).Value = employee.Department;
            insertCmd.Parameters.Add("@HireDate", SqlDbType.DateTime).Value = employee.HireDate;
            insertCmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = employee.Salary;

            await insertCmd.ExecuteNonQueryAsync();

            return true;
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
