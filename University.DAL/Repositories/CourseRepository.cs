using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class CourseRepository
    {
        private readonly appDBcontext _context;

        public CourseRepository(appDBcontext dbcontext)
        {
            _context = dbcontext;
        }

        public List<Course> GetAll()
        {
            return _context.CourseTable.ToList();
        }

        public List<DropdownItem> DropdownList(string itemName)
        {
            List<DropdownItem> items = new List<DropdownItem>();

            if(itemName=="department")
            {
                foreach (var item in _context.DepartmentTable)
                {
                    items.Add( new DropdownItem { Text = item.DepartmentName, Value = item.DepartmentName });
                }
            }
            else
            {
                foreach (var item in _context.TeacherTable)
                {
                    items.Add(
                        new DropdownItem { Text = $"{item.TeacherName} {item.TeacherId}", Value = $"{item.TeacherName}({item.TeacherId})"} );
                }
            }

            return items;
        }
        
        public void Add(Course course)
        {
            _context.CourseTable.Add(course);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Course?> FindAsync(int id)
        {
            return await _context.CourseTable.FindAsync(id);
        }

        public void Remove(Course course)
        {
            _context.CourseTable.Remove(course);
        }

        public void Update(Course course)
        {
            _context.CourseTable.Update(course);
        }
    }
}
