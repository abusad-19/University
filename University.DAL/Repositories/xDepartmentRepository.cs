using Microsoft.EntityFrameworkCore;
using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class xDepartmentRepository
    {
        private readonly appDBcontext _context;
        public xDepartmentRepository(appDBcontext context)
        {
            _context = context;
        }

        public Department? FindDepartment(int departmentCode)
        {
            return _context.DepartmentTable.FirstOrDefault(d => d.DepartmentCode == departmentCode);
        }

        public List<Teacher> GetAllTeachersByDepartment(string departmentName)
        {
            return _context.TeacherTable.Where(t=>t.Department==departmentName).ToList();
        }

        public List<Course> GetAllCoursesByDepartment(string departmentName)
        {
            return _context.CourseTable.Where(c=>c.Department==departmentName).ToList();
        }

        public List<Student> GetAllStudentsByDepartmentAndYear(string departmentName, string year)
        {
            return _context.StudentTable.FromSqlInterpolated
                ($"select * from StudentTable where Department={departmentName} and Year={year} order by StudentId").ToList();
        }

        public Student? GetStudentById(int studentId)
        {
            return _context.StudentTable.FirstOrDefault(s => s.StudentId == studentId);
        }

        public List<Course> GetAllCoursesByDepartmentAndYear(string departmentName, string year)
        {
            return _context.CourseTable.FromSqlInterpolated
                ($"select * from CourseTable where Department={departmentName} and Year={year}").ToList();
        }

        public List<StudentResult>? GetAllEnrolledCourses(int studentId,string year)
        {
            return _context.StudentResultTable.FromSqlInterpolated
                ($"select * from StudentResultTable where StudentId={studentId} and Year={year}").ToList();
        }

        public StudentResultForYear? GetYearResult(int studentId, string year)
        {
            return _context.StudentResultForYearTable.FromSqlInterpolated
                ($"select * from StudentResultForYearTable where StudentId={studentId} and Year={year}").FirstOrDefault();
        }

        public StudentResult? GetByStudentIdAndCourseId(int studentId,int? courseCode)
        {
            if(courseCode is null) 
                return null;
            return _context.StudentResultTable.FromSqlInterpolated($"select * from StudentResultTable where StudentId={studentId} and CourseCode={courseCode}").FirstOrDefault();
        }

        public void UpdateStudentResult(StudentResult pupil)
        {
            _context.StudentResultTable.Update(pupil);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateStudent(Student student)
        {
            _context.StudentTable.Update(student);
        }

        public void AddYearFinalResult(StudentResultForYear studentResult)
        {
            _context.StudentResultForYearTable.Add(studentResult);
        }

        public void UpdateYearFinalResult(StudentResultForYear studentResult)
        {
            _context.StudentResultForYearTable.Update(studentResult);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
