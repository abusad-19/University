using Microsoft.EntityFrameworkCore;
using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class EmployeeBLL: IEmployeeBLL
    {
        private readonly EmployeeRepository _employeeRepository;
        public EmployeeBLL(
            EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public List<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public List<string> GetAllDepartmentName()
        {
            List<string> departmentName = new List<string>();
            var departments= _employeeRepository.GetAllDepartment();
            foreach(var department in departments)
            {
                departmentName.Add(department.DepartmentName!);
            }
            return departmentName;
        }

        public void AddEmployee(Employee employee)
        {
            _employeeRepository.AddEmployee(employee);
            _employeeRepository.SaveChanges();
        }

        public Employee? GetEmployeeById(int id)
        {
            return _employeeRepository.GetEmployeeById(id);
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.UpdateEmployee(employee);
            _employeeRepository.SaveChanges();
        }

        public void DeleteEmployee(Employee employee)
        {
            _employeeRepository.DeleteEmployee(employee);
            _employeeRepository.SaveChanges();
        }

        public Employee? GetEmployeeByEmployeeID(int employeeId)
        {
            return _employeeRepository.GetEmployeeByEmployeeID(employeeId);
        }
    }
}
