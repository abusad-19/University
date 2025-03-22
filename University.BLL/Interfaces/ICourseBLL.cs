using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface ICourseBLL
    {
        List<Course> GetCourseList();
        List<DropdownItem> GetDropdownList(string contentName);
        Task AddCourseAsync(Course course);
        Task<Course?> FindAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Course course);
    }
}
