using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IEmployeeBLL
    {
        List<Employee> GetAll();
        List<string> GetAllDepartmentName();
        void AddEmployee(Employee employee);
        Employee? GetEmployeeById(int id);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
