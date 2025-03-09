using Microsoft.AspNetCore.Mvc;
using University.BLL.Interfaces;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class xDepartmentController : Controller
    {
        private readonly IxDepartmentBLL _xDepartmentBll;
        public xDepartmentController(IxDepartmentBLL bll)
        {
            _xDepartmentBll = bll;
        }

        public IActionResult Index(int departmentCode)
        { 
            if(departmentCode is 0)
                return NotFound();

            var department = _xDepartmentBll.FindDepartment(departmentCode);
            if(department == null)
                return NotFound();

            ViewBag.deptId = departmentCode;
            ViewBag.department = department.DepartmentName;
            return View();
        }

        public IActionResult ShowTeacher(int departmentCode)
        {
            if(departmentCode is 0)
                return NotFound();

            var dept = _xDepartmentBll.FindDepartment(departmentCode);
            if(dept is null || dept.DepartmentName is null)
                return NotFound();
            //string departmentName = dept.DepartmentName;

            ViewBag.teachers = _xDepartmentBll.GetAllTeachersByDepartment(dept.DepartmentName);
            ViewBag.xdepartment=dept.DepartmentName;
            return View();
        }

        public IActionResult ShowCourse(int departmentCode)
        {
            if(departmentCode is 0)
                return NotFound();

            var dept = _xDepartmentBll.FindDepartment(departmentCode);
            if(dept is null || dept.DepartmentName is null)
                return NotFound();

            //var selectedCourses = _context.CourseTable.Where(c => c.Department == departmentName).ToList();
            ViewBag.Courses = _xDepartmentBll.GetAllCoursesByDepartment(dept.DepartmentName);
            ViewBag.xdepartment = dept.DepartmentName;
            return View();
        }

        public IActionResult ShowStudent(int departmentCode) 
        {
            ViewBag.dept = departmentCode;
            return View();
        }

        public IActionResult ShowStudentsYearWise(int deptId,string Year)
        {
            if (deptId is 0 || Year is null)
                return NotFound();

            var dept = _xDepartmentBll.FindDepartment(deptId);
            if(dept is null || dept.DepartmentName is null)
                return NotFound();

            ViewBag.studentYearWise = _xDepartmentBll.GetAllStudentsByDepartmentAndYear(dept.DepartmentName,Year);
            ViewBag.year = Year;
            return View();
        }

        public IActionResult ResultPerYearForEachStudent(int studentId )
        {
            if(studentId is 0)
                return NotFound();

            var student = _xDepartmentBll.GetStudentById(studentId);
            if(student is null)
                return NotFound();

            ViewBag.student = student.StudentName;
            ViewBag.year = student.Year;
            ViewBag.studentId = studentId;
            return View();
        }

        public IActionResult ResultCalculatorForEachCourse(int studentId, string year, bool isWrong)
        {
            var temp= _xDepartmentBll.GetAllCoursesForEachStudent(studentId, year);
            if(temp is null) 
                return NotFound();
            
            if(temp.HasValue)
            {
                ViewBag.studentName = temp.Value.Item1;
                ViewBag.allEnrolled=temp.Value.Item2;
                ViewBag.isMarkNull = temp.Value.Item3;
                ViewBag.needUpdate=temp.Value.Item4;
                ViewBag.recentYearFinalResult=temp.Value.Item5;
                ViewBag.myCourses=temp.Value.Item6;
                ViewBag.studentId=studentId;
                ViewBag.year = year;
            }

            if (isWrong is true)
                ViewBag.isWrong = true;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResultCalculatorForEachCourse(StudentResult course)
        {
            if(course.Mark < 0 || course.StudentId<=0 || course.CourseCode<=0 || course.Year is null)
                return NotFound();

            await _xDepartmentBll.SaveResultForSingleCourseAsync(course);
            return RedirectToAction(nameof(ResultCalculatorForEachCourse), new { studentId = course.StudentId,year=course.Year, isWrong = false });
        }

        public IActionResult GenerateYearFinalResult(int studentId)
        {
            if(studentId <=0)
                return NotFound();

            var temp=_xDepartmentBll.GenerateYearFinalResult(studentId);

            if(temp.HasValue)
            {
                ViewBag.year=temp.Value.Item1;
                ViewBag.courses=temp.Value.Item2;
                ViewBag.yearFinal=temp.Value.Item3;
            }
            else
            {
                return NotFound();
            }
      
            ViewBag.studentId = studentId;
            return View();
        }
            
        public async Task<IActionResult> UpdateYearResult(int id, string year)//here id is studentId
        {
            if(id<=0)
                return NotFound();

            var temp=await _xDepartmentBll.UpdateYearResultAsync(id, year);
            if(temp is true) 
            {
                return RedirectToAction(nameof(ResultCalculatorForEachCourse), 
                    new { studentId = id, year = year, isWrong = true });
            }
            else
            {
                return RedirectToAction(nameof(ResultCalculatorForEachCourse), 
                    new { studentId = id, year = year, isWrong = false });
            }
        }


    }
}
