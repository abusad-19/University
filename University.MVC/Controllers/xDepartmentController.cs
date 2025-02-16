using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class xDepartmentController : Controller
    {
        private readonly appDBcontext _context;
        public xDepartmentController(appDBcontext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        { 
            ViewBag.Id = id;
            ViewBag.department=findDepartmentName(id);
            return View();
        }

        public string findDepartmentName(int id) 
        {
            if(id == null)
                return null;
            var target=_context.DepartmentTable.Where(p=>p.DepartmentCode==id).ToList();
            if (target == null)
                return null;
            return target[0].DepartmentName;
        }

        public async Task<IActionResult> ShowTeacher(int id)
        {
            //compiler give me an error for await... what is the wrong????
            string departmentName = findDepartmentName(id);
            if(departmentName == null)
                return NotFound();

            var selectedTeachers = _context.TeacherTable.Where(p => p.Department == departmentName).ToList();
            ViewBag.teachers = selectedTeachers;
            ViewBag.xdepartment=departmentName;
            return View();
        }

        public async Task<IActionResult> ShowCourse(int id)
        {
            string  departmentName=findDepartmentName(id);
            if(departmentName == null)
                return NotFound();

            var selectedCourses = _context.CourseTable.Where(c => c.Department == departmentName).ToList();
            ViewBag.Courses = selectedCourses;
            ViewBag.xdepartment = departmentName;
            return View();
        }

        public IActionResult ShowStudent(int id) 
        {
            ViewBag.dept = id;
            return View();
        }

        public IActionResult ShowStudentsYearWise(int deptId,string Year)
        {
            var dept=_context.DepartmentTable.Where(c => c.DepartmentCode == deptId).ToList();
            var targetStudent=_context.StudentTable.FromSqlInterpolated($"select * from StudentTable where Department={dept[0].DepartmentName} and Year={Year} order by StudentId").ToList();
            ViewBag.studentYearWise = targetStudent;
            ViewBag.year = Year;
            return View();
        }

        public IActionResult ResultCalculatorForEachCourse(int id)
        {
            var OnePupilEnrolledCourses = _context.StudentResultTable.Where(d => d.StudentId == id).ToList();
            ViewBag.myCourses = OnePupilEnrolledCourses;
            return View();
        }

        private float PointCalculator(int mark)
        {
            if (mark >= 80)
                return 4.0F;
            else if (mark >= 75 && mark < 80)
                return 3.75F;
            else if (mark >= 70 && mark < 75)
                return 3.50F;
            else if (mark >= 65 && mark < 70)
                return 3.25F;
            else if (mark >= 60 && mark < 65)
                return 3.00F;
            else if (mark >= 55 && mark < 60)
                return 2.75F;
            else if (mark >= 50 && mark < 60)
                return 2.50F;
            else if (mark >= 45 && mark < 50)
                return 2.25F;
            else if (mark >= 40 && mark < 45)
                return 2.00F;
            else
                return 0;
        }

        [HttpPost]
        public IActionResult ResultCalculatorForEachCourse(int id, int Mark, string courseName)
        {
            //var OnePupilEnrolledCourses = _context.StudentResultTable.Where(d => d.StudentId == id).ToList();
            //ViewBag.myCourses = OnePupilEnrolledCourses;
            var pupilSet = _context.StudentResultTable.FromSqlInterpolated($"select * from StudentResultTable where StudentId={id} and CourseName={courseName}").ToList();
            var target = pupilSet[0];
            StudentResult temp= new StudentResult();
            temp.Id = target.Id;
            temp.StudentId = target.StudentId;
            temp.CourseName=target.CourseName;
            temp.CourseCredit=target.CourseCredit;
            temp.Mark = Mark;
            temp.GPA= PointCalculator(Mark);
            //pupilSet.Mark= Mark;
            //pupilSet.GPA = PointCalculator(Mark);
            _context.StudentResultTable.Update(temp);
            return RedirectToAction(nameof(ResultCalculatorForEachCourse), new { id=id});
        }
    }
}
