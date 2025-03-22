using Microsoft.EntityFrameworkCore;
using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class xStudentRepository
    {
        private readonly appDBcontext _context;
        public xStudentRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<LendBook> GetIssuedBooks(int studentId)
        {
            return _context.LendBookTable.Where(b => b.StudentId == studentId).ToList();
        }

        public Student? GetStudentByStudentId(int studentId)
        {
            return _context.StudentTable.FirstOrDefault(s=>s.StudentId == studentId);
        }

        public List<Course> GetCoursesByDepartmentAndYear(string department, string year)
        {
            return _context.CourseTable.FromSqlInterpolated
                ($"select * from CourseTable where Department={department} and Year={year}").
                ToList();
        }

        public List<StudentResult> GetEnrolledCourses(int studentId,string? year)
        {
            if(year is null)
            {
                return _context.StudentResultTable.FromSqlInterpolated
                ($"select * from StudentResultTable where StudentId={studentId}").
                ToList();
            }

            return _context.StudentResultTable.FromSqlInterpolated
                ($"select * from StudentResultTable where StudentId={studentId} and Year={year}").
                ToList();
        }

        public Course? GetCourseByCourseId(int courseId)
        {
            return _context.CourseTable.FirstOrDefault(c=>c.CourseCode == courseId);
        }

        public void AddStudentResult(StudentResult studentResult)
        {
            _context.StudentResultTable.Add(studentResult);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public StudentResultForYear? GetStudentResultForYearByIdAndYear(int studentId, string year)
        {
            var temp= _context.StudentResultForYearTable.FromSqlInterpolated
                ($"Select * from StudentResultForYearTable where StudentId={studentId} and Year={year}").
                ToList();
            if(temp.Count is 0)
                return null;
            else
                return temp[0];
        }
    
        public void AddCertificateWithdrawRequest(CertificateWithdrawApprovalHistory request)
        {
            _context.CertificateWithdrawApprovalHistoryTable.Add(request);
        }
    }
}
