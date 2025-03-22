using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface ITeacherBLL
    {
        List<Teacher> GetTeacherList();
        List<DropdownItem> GetTeacherDropdownList();
        Task AddAsync(Teacher teacher);
        Task<Teacher?> FindAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Teacher teacher);
    }
}
