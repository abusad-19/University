using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.BLL.Interfaces;

namespace University.MVC.Controllers
{
    public class xStudentController : Controller
    {
        private readonly IxStudentBLL _xStudentBll;
        public xStudentController(IxStudentBLL bll)
        {
            _xStudentBll = bll;
        }

        [Authorize(Policy= "CanReadStudentProfile")]
        public IActionResult Index(int id)//here id is studentId
        {
            var student=_xStudentBll.GetStudentByStudentId(id);
            if(student is null)
                return NotFound();
            ViewBag.studentName = student.StudentName;
            ViewBag.stdId = id;
            return View();
        }

        [Authorize(Policy = "CanReadStudentProfile")]
        public IActionResult CourseCanBeEnroll(int id)
        {
            if(id<=0) 
                return NotFound();
            var student = _xStudentBll.GetStudentByStudentId(id);
            if(student is null)
                return NotFound();

            var temp=_xStudentBll.GetEnrolledAndMayBeEnrolledCourse(student); 
            ViewBag.courses=temp.Item1;
            ViewBag.alreadyEnrolled = temp.Item2;

            ViewBag.student = id;
            return View();
        }

        [Authorize(Policy = "CanReadStudentProfile")]
        public IActionResult EnrollCourse(int pupilId, int courseId) 
        {
            if(pupilId<=0 || courseId<=0)
                return NotFound();
            
            var isCompleted=_xStudentBll.EnrollCourse(pupilId, courseId);
            if(isCompleted is false)
                return NotFound();

            return RedirectToAction(nameof(CourseCanBeEnroll),new {id=pupilId});
        }

        [Authorize(Policy = "CanReadStudentProfile")]
        public IActionResult MyEnrolledCourses(int id)//here id is studentId
        {
            if(id<=0)
                return NotFound();
            var myEnrolledCourses=_xStudentBll.GetMyEnrolledCourses(id);
            ViewBag.myCourses=myEnrolledCourses;
            return View();
        }

        [Authorize(Policy = "CanReadStudentProfile")]
        public IActionResult ShowResultYearWise(int id)//here id is studentId
        {
            if(id<=0)
                return NotFound();

            var student= _xStudentBll.GetStudentByStudentId(id);
            if(student is null)
                return NotFound();

            ViewBag.student = student!.StudentName;
            ViewBag.year = student.Year;
            ViewBag.studentId = id;
            return View();

        }

        [Authorize(Policy = "CanReadStudentProfile")]
        public IActionResult YearFinalResult(int id, string YearName)//here id is studentId
        {
            if (id<=0)
                return NotFound();

            var temp=_xStudentBll.GetYearFinalResult(id, YearName);

            if(temp is null)
            {
                ViewBag.result = true;
                return View();
            }

            ViewBag.yearFinal = temp.Value.Item1;
            var courses=temp.Value.Item2;

            return View(courses);
        }

        [Authorize(Policy = "CanReadStudentProfile")]
        public IActionResult LibraryIssuedBooks(int id)//here id is studentId
        {
            if (id <= 0)
                return NotFound();
            var books = _xStudentBll.GetIssuedBooks(id);
            return View(books);
        }

        //[Authorize(Policy = "CanReadStudentProfile")]
        //public IActionResult CertificateWithdraw(int id)
        //{
        //    if(id<=0)
        //        return NotFound();

        //    var student=_xStudentBll.GetStudentByStudentId(id);
        //    if(student is null)
        //        return NotFound();

        //}
    }
}
