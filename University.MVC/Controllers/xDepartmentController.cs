using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.BLL.Services;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class xDepartmentController : Controller
    {
        private readonly appDBcontext _context;
        private readonly xDepartmentBLL _xDepartmentBll;
        public xDepartmentController(appDBcontext context, xDepartmentBLL bll)
        {
            _context = context;
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
            if(course.Mark == 0 || course.StudentId==0 || course.CourseCode==0 || course.Year is null)
                return NotFound();

            await _xDepartmentBll.SaveResultForSingleCourseAsync(course);
            return RedirectToAction(nameof(ResultCalculatorForEachCourse), new { studentId = course.StudentId,year=course.Year, isWrong = false });
        }

        public IActionResult GenerateYearFinalResult(int studentId)
        {
            if(studentId is 0)
                return NotFound();

            var temp=_xDepartmentBll.GenerateYearFinalResult(studentId);
            if(temp.HasValue)
            {
                ViewBag.year=temp.Value.Item1;
                ViewBag.courses=temp.Value.Item2;
                ViewBag.yearFinal=temp.Value.Item3;
            }
      
            ViewBag.studentId = studentId;
            return View();
        }
            
        public IActionResult UpdateYearResult(int id, string year)
        {
            var yearFinal = _context.StudentResultForYearTable.FromSqlInterpolated
                ($"select * from StudentResultForYearTable where StudentId={id} and Year={year}").ToList();
            var courses=_context.StudentResultTable.FromSqlInterpolated
                ($"select * from StudentResultTable where StudentId={id} and Year={year}").ToList();

            if (yearFinal[0].GPApoint==0)
            {
                var target = _context.StudentTable.FromSqlInterpolated
                    ($"select * from StudentTable where StudentId={id} and Year={year}").ToList()[0];

                float up = 0;
                float down = 0;
                StudentResult labCourse= new StudentResult();
                int lossCredit = 0;
                foreach (var course in courses)
                {
                    up = up + course.CourseCredit * course.GPA ?? 0;
                    down = down + course.CourseCredit;

                    if (course.IsLab is true)
                        labCourse = course;
                    if (course.GPA == 0)
                        lossCredit = lossCredit + course.CourseCredit;
                }

                if(labCourse.GPA != 0 && lossCredit<10)
                {
                    yearFinal[0].GPApoint = up / down;
                    _context.StudentResultForYearTable.Update(yearFinal[0]);

                    if (target.Year == "First Year")
                        target.Year = "Second Year";
                    else if (target.Year == "Second Year")
                        target.Year = "Third Year";
                    else if (target.Year == "Third Year")
                        target.Year = "Fourth Year";
                    else
                        target.Year = "Master's";
                    _context.StudentTable.Update(target);
                }
            }
            else
            {
                float up = 0;
                float down = 0;
                StudentResult labCourse = new StudentResult();
                int lossCredit = 0;
                foreach (var course in courses)
                {
                    up = up + course.CourseCredit * course.GPA ?? 0;
                    down = down + course.CourseCredit;

                    if (course.IsLab is true)
                        labCourse = course;
                    if (course.GPA == 0)
                        lossCredit = lossCredit + course.CourseCredit;
                }

                if (labCourse.GPA == 0 || lossCredit > 10)
                {
                    yearFinal[0].GPApoint = 0;

                    //finding the target student
                    Student target= new Student();
                    if (year == "First Year")
                    {
                        var mayBeTarget = _context.StudentTable.FromSqlInterpolated
                            ($"select * from StudentTable where StudentId={id} and Year={"Second Year"}").ToList();
                        if(mayBeTarget.Count==0)
                        {
                            target = null;
                        }
                        else
                        {
                            target = mayBeTarget[0];
                        }
                    }
                    else if (year == "Second Year")
                    {
                        var mayBeTarget = _context.StudentTable.FromSqlInterpolated
                            ($"select * from StudentTable where StudentId={id} and Year={"Third Year"}").ToList();
                        if (mayBeTarget.Count == 0)
                        {
                            target = null;
                        }
                        else
                        {
                            target = mayBeTarget[0];
                        }
                    }
                    else if (year == "Third Year")
                    {
                        var mayBeTarget = _context.StudentTable.FromSqlInterpolated
                            ($"select * from StudentTable where StudentId={id} and Year={"Fourth Year"}").ToList();
                        if (mayBeTarget.Count == 0)
                        {
                            target = null;
                        }
                        else
                        {
                            target = mayBeTarget[0];
                        }
                    }
                    else
                    {
                        var mayBeTarget = _context.StudentTable.FromSqlInterpolated
                            ($"select * from StudentTable where StudentId={id} and Year={"Master's"}").ToList();
                        if (mayBeTarget.Count == 0)
                        {
                            target = null;
                        }
                        else
                        {
                            target = mayBeTarget[0];
                        }
                    }

                    

                    if (target is null)
                    {
                        return RedirectToAction(nameof(ResultCalculatorForEachCourse), new { studentId = id, year = year, isWrong=true});
                    }
                        
                    
                    //give the target student Demossion
                    if (target.Year == "Second Year")
                        target.Year = "First Year";
                    else if (target.Year == "Third Year")
                        target.Year = "Second Year";
                    else if (target.Year == "Fourth Year")
                        target.Year = "Third Year";
                    else
                        target.Year = "Fourth Year";
                    _context.StudentTable.Update(target);
                }
                else
                {
                    yearFinal[0].GPApoint = up / down;   
                }
                _context.StudentResultForYearTable.Update(yearFinal[0]);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(ResultCalculatorForEachCourse), new {studentId=id,year=year, isWrong=false});
        }
    }
}
