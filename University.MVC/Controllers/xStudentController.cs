using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class xStudentController : Controller
    {
        private readonly appDBcontext _context;

        public xStudentController(appDBcontext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var student=_context.StudentTable.Where(s=>s.StudentId==id).ToList();
            if(student==null)
                return NotFound();
            ViewBag.studentName = student[0].StudentName;
            ViewBag.stdId = id;
            return View();
        }
        public IActionResult CourseCanBeEnroll(int id)
        {
            var temp=_context.StudentTable.Where(s=>s.StudentId == id).ToList();
            var student = temp[0];
            if(temp==null)
                return NotFound();
            var mayBeLegalCourses=_context.CourseTable.FromSqlInterpolated($"select * from CourseTable where Department={student.Department} and Year={student.Year}").ToList();
            var enrolledCourses = _context.StudentResultTable.FromSqlInterpolated($"select * from StudentResultTable where StudentId={student.StudentId} and Year={student.Year}").ToList();
            List<Course> legalCourses = new List<Course>();
            List<Course> alreadyEnrolled=new List<Course>();//for only using in view
            foreach (var item1 in mayBeLegalCourses)
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
                    legalCourses.Add(item1);
                }
                else
                {
                    alreadyEnrolled.Add(item1);
                }
            }

            ViewBag.courses=legalCourses;
            ViewBag.alreadyEnrolled = alreadyEnrolled;
            ViewBag.student = id;
            return View();
        }

        public async Task<IActionResult> EnrollCourse(int pupilId, int courseId) 
        {
            var courseInfo=_context.CourseTable.Where(c=>c.CourseCode==courseId).ToList();
            StudentResult temp = new StudentResult();
            temp.StudentId = pupilId;
            temp.CourseName= courseInfo[0].CourseName;
            temp.CourseCode= courseInfo[0].CourseCode;
            temp.CourseCredit= courseInfo[0].Credit;
            temp.Year = courseInfo[0].Year;
            temp.IsLab= courseInfo[0].IsLab;
            _context.StudentResultTable.Add(temp);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CourseCanBeEnroll),new {id=pupilId});
        }

        public IActionResult MyEnrolledCourses(int id)
        {
            var myEnrolledCourses=_context.StudentResultTable.Where(d=>d.StudentId == id).ToList();
            ViewBag.myCourses=myEnrolledCourses;
            return View();
        }

        public IActionResult ShowResultYearWise(int id)
        {
            var student= _context.StudentTable.Where(s=>s.StudentId==id).ToList();
            ViewBag.student = student[0].StudentName;
            ViewBag.year = student[0].Year;
            ViewBag.studentId = id;
            return View();

        }

        public IActionResult YearFinalResult(int id, string YearName)
        {
            var pupil = _context.StudentTable.Where(c => c.StudentId == id).ToList();
            var isResultCreated=_context.StudentResultForYearTable.FromSqlInterpolated
                ($"Select * from StudentResultForYearTable where StudentId={id} and Year={YearName}").ToList();
            if(isResultCreated.Count== 0)
            {
                ViewBag.result = true;
                return View();
            }

            ViewBag.yearFinal = isResultCreated[0].GPApoint;

            var courses=_context.StudentResultTable.FromSqlInterpolated
                ($"Select * from StudentResultTable where StudentId={id} and Year={YearName}").ToList();
                
            return View(courses);
        }
    }
}
