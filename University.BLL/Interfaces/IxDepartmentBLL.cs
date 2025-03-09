using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IxDepartmentBLL
    {
        Department? FindDepartment(int departmentCode);
        List<Teacher> GetAllTeachersByDepartment(string departmentName);
        List<Course> GetAllCoursesByDepartment(string departmentName);
        List<Student>? GetAllStudentsByDepartmentAndYear(string deptName, string year);
        Student? GetStudentById(int studentId);
        (string, bool, bool, bool, float?, List<StudentResult>)? GetAllCoursesForEachStudent(int studentId, string year);
        Task SaveResultForSingleCourseAsync(StudentResult course);
        (string, List<StudentResult>, float)? GenerateYearFinalResult(int studentId);
        Task<bool> UpdateYearResultAsync(int studentId, string year);
    }
}
