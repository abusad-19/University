using Microsoft.EntityFrameworkCore;
using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class xStudentBLL:IxStudentBLL
    {
        private readonly xStudentRepository _repository;
        public xStudentBLL(xStudentRepository repository)
        {
            _repository = repository;
        }

        public List<LendBook> GetIssuedBooks(int studentId)
        {
            return _repository.GetIssuedBooks(studentId);
        }

        public Student? GetStudentByStudentId(int studentId)
        {
            return _repository.GetStudentByStudentId(studentId);
        }

        public (List<Course>,List<Course>) GetEnrolledAndMayBeEnrolledCourse(Student student)
        {
            var courses = _repository.GetCoursesByDepartmentAndYear(student.Department!, student.Year);
            var enrolledCourses = _repository.GetEnrolledCourses(student.StudentId, student.Year);
             
            List<Course> disEnrolled = new List<Course>();
            List<Course> enrolled = new List<Course>();//for only using in view
            foreach (var item1 in courses)
            {
                bool isEnrolled = false;
                foreach (var item2 in enrolledCourses)
                {
                    if (item1.CourseName == item2.CourseName)
                    {
                        isEnrolled = true;
                        break;
                    }
                }

                if (isEnrolled is false)
                {
                    disEnrolled.Add(item1);
                }
                else
                {
                    enrolled.Add(item1);
                }
            }
            return (disEnrolled, enrolled);
        }

        public bool EnrollCourse(int pupilId, int courseId)
        {
            var student=_repository.GetStudentByStudentId(pupilId);
            if (student is null)
            {
                return false;
            }
            var course=_repository.GetCourseByCourseId(courseId);
            if (course is null)
            {
                return false;
            }
            
            StudentResult temp = new StudentResult();
            temp.StudentId = pupilId;
            temp.CourseName = course.CourseName;
            temp.CourseCode = course.CourseCode;
            temp.CourseCredit = course.Credit;
            temp.Year = course.Year;
            temp.IsLab = course.IsLab;
            _repository.AddStudentResult(temp);
            _repository.SaveChanges();
            return true;
        }

        public List<StudentResult> GetMyEnrolledCourses(int studentId)
        {
            return _repository.GetEnrolledCourses(studentId,null);
        }

        public (float,List<StudentResult>)? GetYearFinalResult(int studentId,string year)
        {
            var yearResult=_repository!.GetStudentResultForYearByIdAndYear(studentId,year);
            if (yearResult is null)
                return null;

            var courses = _repository.GetEnrolledCourses(studentId, year);
            return (yearResult.GPApoint, courses);
        }

        //public void CreateCertificateWithdrawRequest(int studentId)
        //{
        //    var request = new CertificateWithdrawApprovalHistory();
        //    request.StudentId = studentId;
        //    request.RequestCreated= DateTime.Now;
        //    _repository.AddCertificateWithdrawRequest(request);
        //    _repository.SaveChanges();
        //}
    }
}
