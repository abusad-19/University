using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class DepartmentRepository
    {
        private readonly appDBcontext _context;
        public DepartmentRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<Department> GetAll()
        {
            return _context.DepartmentTable.ToList();
        }

        public void Add(Department department)
        {
             _context.DepartmentTable.Add(department);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Department?> FindAsync(int id)
        {
            return await _context.DepartmentTable.FindAsync(id);
        }

        public void Remove(Department department)
        {
            _context.DepartmentTable.Remove(department);
        }

        public void Update(Department department)
        {
            _context.DepartmentTable.Update(department);
        }
    }
}
