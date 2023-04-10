using Microsoft.EntityFrameworkCore;
using VismaTask.Data;

namespace VismaTask.Services
{
    public class EmployeeService : IEmployeeService
    {
        private EmployeeDbContext db;
        public EmployeeService(EmployeeDbContext employeeDbContext) {
            db = employeeDbContext;
        }
        public async Task<Employee?> AddEmployee(Employee employee)
        {
            if(employee == null)
            {
                return default;
            }
            await db.AddAsync(employee);
            await db.SaveChangesAsync();
            
            return employee;            
        }

        public async Task<bool?> DeleteEmployee(int id)
        {
            Employee? employee = await db.Employees.FindAsync(id);
            if(employee == null)
            {
                return default;
            }
            db.Employees.Remove(employee);
            await db.SaveChangesAsync();

            return true;            
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await db.Employees.ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByBossId(int id)
        {
            if (id == null)
            {
                return null;
            }
            var foundEmployees = await db.Employees.Where(x=>x.BossId == id).ToListAsync();

            if(foundEmployees is null)
            {
                return default;
            }

            return foundEmployees;
        }

        public async Task<Employee?> GetByID(int id)
        {
            if (id == null)
            {
                return null!;
            }
            var foundEmployee = await db.Employees.FindAsync(id);

            if(foundEmployee is null)
            {
                return default;
            }

            return foundEmployee;
        }

        public async Task<IEnumerable<Employee>> GetByNameAndDate(string name, DateTime startDate, DateTime endDate)
        {
            var employees = await db.Employees.Where(x=> (x.FirstName.Contains(name) || x.LastName.Contains(name)) && x.Birthdate>=startDate && x.Birthdate.Date <=endDate.Date ).ToListAsync();
            if(!employees.Any())
            {
                return default;
            }
            return employees;
        }

        public async Task<(int, decimal)> GetCountAndAvgSalary(string role)
        {   
            var employeesByRole = await db.Employees.Where(e => e.Role == role).ToListAsync();
            int count = employeesByRole.Count;

            if(count == 0)
            {
                return (0, 0);
            }
            decimal averageSalary = employeesByRole.Average(e => e.CurrentSalary);

            return (count, averageSalary);
        }

        public async Task<Employee?> UpdateEmployee(Employee employee)
        {
            db.Employees.Update(employee);
            await db.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateSalary(Employee employee, int id)
        {
            db.Employees.Update(employee);
            await db.SaveChangesAsync();
            return employee;
        }
    }
}