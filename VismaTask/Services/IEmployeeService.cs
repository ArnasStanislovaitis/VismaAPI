namespace VismaTask.Services
{
    public interface IEmployeeService
    {
        Task<Employee?> GetByID(int id);
        Task<IEnumerable<Employee?>> GetByNameAndDate(string name, DateTime startDate,DateTime endDate);
        Task<IEnumerable<Employee?>> GetAll();
        Task<IEnumerable<Employee?>> GetByBossId(int id);
        Task<(int, decimal)> GetCountAndAvgSalary(string role);
        Task<Employee?> AddEmployee(Employee employee);
        Task<Employee?> UpdateEmployee(Employee employee);
        Task<Employee?> UpdateSalary(Employee employee, int id);
        Task<bool?> DeleteEmployee(int id);
    }
}