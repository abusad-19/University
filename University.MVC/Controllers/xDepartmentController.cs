using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult ResultPerYearForEachStudent(int id )
        {
            var student = _context.StudentTable.Where(s => s.StudentId == id).ToList();
            ViewBag.student = student[0].StudentName;
            ViewBag.year = student[0].Year;
            ViewBag.studentId = id;
            return View();
        }

        public IActionResult ResultCalculatorForEachCourse(int id, string year, bool isWrong)// isWrong is 
        {
            var temp = _context.StudentTable.Where(s => s.StudentId == id).ToList();
            ViewBag.studentName = temp[0].StudentName;
            var coursesOfYear = _context.CourseTable.FromSqlInterpolated($"select * from CourseTable where Department={temp[0].Department} and Year={year}").ToList();
            var OnePupilEnrolledCourses = _context.StudentResultTable.FromSqlInterpolated($"select * from StudentResultTable where StudentId={id} and Year={year}").ToList();

            if(coursesOfYear.Count==OnePupilEnrolledCourses.Count)
            {
                ViewBag.allEnrolled = true;
                foreach(var item in OnePupilEnrolledCourses)
                {
                    if(item.Mark is null)
                    {
                        ViewBag.isMarkNull=true;
                        break;
                    }
                }
            }

            var isItAddedOnce = _context.StudentResultForYearTable.FromSqlInterpolated
                ($"select * from StudentResultForYearTable where StudentId={id} and Year={year}").ToList();
            if(isItAddedOnce.Count==1) 
            {
                ViewBag.needUpdate=true;
                ViewBag.recentYearFinalResult = isItAddedOnce[0].GPApoint;
            }
            ViewBag.myCourses = OnePupilEnrolledCourses;
            ViewBag.studentName = temp[0].StudentName;
            ViewBag.studentId = temp[0].StudentId;
            ViewBag.year = year;
            if(isWrong is true)
                ViewBag.isWrong = true;
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
        public async Task<IActionResult> ResultCalculatorForEachCourse(StudentResult course)
        {
            var pupilSet = _context.StudentResultTable.FromSqlInterpolated($"select * from StudentResultTable where StudentId={course.StudentId} and CourseCode={course.CourseCode}").ToList();
            var target = pupilSet[0];
            target.Mark=course.Mark;
            if(target.CourseCredit==2)
            {
                target.GPA = PointCalculator(2*course.Mark ?? 0);
            }
            else
            {
                target.GPA = PointCalculator(course.Mark ?? 0);
            }
            
            _context.StudentResultTable.Update(target);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ResultCalculatorForEachCourse), new { id=course.StudentId,year=target.Year, isWrong = false });
        }

        public IActionResult GenerateYearFinalResult(int id )
        {
            var pupil=_context.StudentTable.Where(s=>s.StudentId==id).ToList();
            var target = pupil[0];
            ViewBag.year=target.Year;
            var courses = _context.StudentResultTable.FromSqlInterpolated($"select * from StudentResultTable where StudentId={id} and Year={target.Year}").ToList();
               
            float up=0;
            float down=0;
            int lossCredit = 0;
            StudentResult Lab=new StudentResult();
            foreach(var course in courses )
            {
                up=up+course.CourseCredit*course.GPA ?? 0;
                down = down + course.CourseCredit;
                if (course.IsLab is true)
                    Lab = course;
                if (course.GPA == 0)
                    lossCredit = course.CourseCredit;
            }
            StudentResultForYear yearFinal = new StudentResultForYear();
            yearFinal.StudentId = id;
            yearFinal.Year = target.Year;
            if (lossCredit > 10 || Lab.GPA == 0)
            {
                yearFinal.GPApoint = 0;
            }
            else
            {
                yearFinal.GPApoint = up / down;
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
            _context.StudentResultForYearTable.Add(yearFinal);
            _context.SaveChanges();
            ViewBag.courses= courses;
            ViewBag.yearFinal = yearFinal.GPApoint;
            ViewBag.studentId = id;
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
                        return RedirectToAction(nameof(ResultCalculatorForEachCourse), new { id = id, year = year, isWrong=true});
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
            return RedirectToAction(nameof(ResultCalculatorForEachCourse), new {id=id,year=year, isWrong=false});
        }
    }
}
