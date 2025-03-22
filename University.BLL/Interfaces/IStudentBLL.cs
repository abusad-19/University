using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IStudentBLL
    {
        Task UpdateAsync(int id, Student pupil);
        List<Student> MakeStudentToList();
        List<DropdownItem> DropdownItemForDepartment();
        void AddStudent(Student pupil);
        Task<Student> FindStudentAsync(int id);
        Task DeleteAsync(int id);
    }
}
