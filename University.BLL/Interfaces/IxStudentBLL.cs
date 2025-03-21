using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IxStudentBLL
    {
        List<LendBook> GetIssuedBooks(int studentId);
        Student? GetStudentByStudentId(int studentId);
        (List<Course>, List<Course>) GetEnrolledAndMayBeEnrolledCourse(Student student);
        bool EnrollCourse(int pupilId, int courseId);
        List<StudentResult> GetMyEnrolledCourses(int studentId);
        (float, List<StudentResult>)? GetYearFinalResult(int studentId, string year);
        //void CreateCertificateWithdrawRequest(int studentId);
    }
}
