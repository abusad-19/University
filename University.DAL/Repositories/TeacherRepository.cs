using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class TeacherRepository
    {
        private readonly appDBcontext _context;
        public TeacherRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<Teacher> GetAllTeacherList()
        {
            return _context.TeacherTable.ToList();
        }

        public List<DropdownItem> GetTeacherDropdownList()
        {
            List<DropdownItem> depts= new List<DropdownItem>();

            foreach(var item in _context.DepartmentTable)
            {
                depts.Add(new DropdownItem { Text = item.DepartmentName, Value = item.DepartmentName });
            }

            return depts;
        }

        public void Add(Teacher teacher)
        {
            _context.TeacherTable.Add(teacher);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Teacher?> FindAsync(int id)
        {
            return await _context.TeacherTable.FindAsync(id);
        }

        public void Remove(Teacher teacher)
        {
            _context.TeacherTable.Remove(teacher);
        }

        public void Update(Teacher teacher)
        {
            _context.TeacherTable.Update(teacher);
        }
    }
}
