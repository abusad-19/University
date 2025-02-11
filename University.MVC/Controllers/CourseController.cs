using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace University.MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly appDBcontext _context;
        public CourseController(appDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.CourseTable.ToList());
        }

        //making dropdown list for department
        public List<SelectListItem> makeDropdownListOfDepartment()
        {
            List<SelectListItem> departments= new List<SelectListItem>();
            foreach(var element in _context.DepartmentTable) 
            {
                departments.Add(new SelectListItem {Text=element.DepartmentName, Value=element.DepartmentName});
            }
            return departments;
        }

        //making dropdown list for teacher
        public List<SelectListItem> makeDropdownListOfTeacher()
        {
            List<SelectListItem> teachers = new List<SelectListItem>();
            foreach (var element in _context.TeacherTable)
            {
                teachers.Add(new SelectListItem { Text = element.TeacherName+" "+element.TeacherId, Value = element.TeacherName });
            }
            return teachers;
        }

        public IActionResult Create()
        {        
            ViewBag.Departments=makeDropdownListOfDepartment();
            ViewBag.Teachers=makeDropdownListOfTeacher();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CourseCode,CourseName,CourseTeacher,Department")]Course course)
        {
            _context.CourseTable.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var target=await _context.CourseTable.FirstOrDefaultAsync(x => x.Id == Id);
            if (target == null)
                return NotFound();
            return View(target);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int Id)
        {
            var target = await _context.CourseTable.FindAsync(Id);
            if(target == null)
                return NotFound();
            _context.CourseTable.Remove(target);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var target= await _context.CourseTable.FirstOrDefaultAsync(x=>x.Id == Id);
            if(target==null)
                return NotFound();
            ViewBag.Departments = makeDropdownListOfDepartment();
            ViewBag.Teachers = makeDropdownListOfTeacher();
            return View(target);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Confirm(int Id, [Bind("CourseCode,CourseName,CourseTeacher,Department")]Course newCourse)
        {
            var oldCourse = await _context.CourseTable.FindAsync(Id);
            if (oldCourse == null)
                return NotFound();
            oldCourse.CourseCode = newCourse.CourseCode;
            oldCourse.CourseName = newCourse.CourseName;
            oldCourse.CourseTeacher = newCourse.CourseTeacher;
            oldCourse.Department = newCourse.Department;
            _context.CourseTable.Update(oldCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
