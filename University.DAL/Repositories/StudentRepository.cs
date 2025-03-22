using Microsoft.EntityFrameworkCore;

using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class StudentRepository
    {
        private readonly appDBcontext _context;
        public StudentRepository(appDBcontext context)
        {
            _context = context ?? throw new Exception();
        }

        public async Task<Student?> FindAsync(int id)
        {
            return await _context.StudentTable.FindAsync(id);
        }

        public void Update(Student pupil)
        {
            _context.StudentTable.Update(pupil);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public List<Student> StudentToList()
        {
            return _context.StudentTable.ToList();
        }

        public List<DropdownItem> DepartmentDropdownItem()
        {
            List<DropdownItem> dropdownItems = new List<DropdownItem>();
            foreach(var item in _context.DepartmentTable)
            {
                dropdownItems.Add(new DropdownItem { Text = item.DepartmentName, Value = item.DepartmentName });
            }
            return dropdownItems;
        }

        public async Task Add(Student pupil)
        {
            _context.StudentTable.Add(pupil);
            await SaveChangesAsync();
        }

        public void Remove(Student pupil)
        {
             _context.StudentTable.Remove(pupil);
        }
    }
}
