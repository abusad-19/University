using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class EmployeeRepository
    {
        private readonly appDBcontext _context;
        public EmployeeRepository(appDBcontext context)
        {
            _context = context ?? throw new Exception();
        }

        public List<Employee> GetAll()
        {
            return _context.EmployeeTable.ToList();
        }

        public List<Department> GetAllDepartment()
        {
            return _context.DepartmentTable.ToList();
        }
        
        public void AddEmployee(Employee employee)
        {
            _context.EmployeeTable.Add(employee);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Employee? GetEmployeeById(int id)
        {
            return _context.EmployeeTable.Find(id);
        }

        public void UpdateEmployee(Employee employee)
        {
            _context.EmployeeTable.Update(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.EmployeeTable.Remove(employee);
        }

        public Employee? GetEmployeeByEmployeeID(int employeeId)
        {
            return _context.EmployeeTable.FirstOrDefault(e => e.EmployeeId == employeeId);
        }
    }
}
