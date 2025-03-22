using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.DAL.Models;
using University.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace University.MVC.Controllers
{
    [Authorize(Policy = "CanCourse_CRUD_operation")]
    public class CourseController : Controller
    {
        private readonly ICourseBLL _courseBLL;
        public CourseController(ICourseBLL courseBLL)
        {
            _courseBLL = courseBLL;
        }
        public IActionResult Index()
        {
            return View(_courseBLL.GetCourseList());
        }

        //making dropdown list of department and teacher
        public List<SelectListItem> makeDropdownList(string contentname)
        {
            List<SelectListItem> dropdownList= new List<SelectListItem>();
            var contentList= _courseBLL.GetDropdownList(contentname);
            foreach(var element in contentList) 
            {
                dropdownList.Add(new SelectListItem {Text=element.Text, Value=element.Value});
            }
            return dropdownList;
        }

        public IActionResult Create()
        {        
            ViewBag.Departments=makeDropdownList("department");
            ViewBag.Teachers=makeDropdownList("teacher");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CourseCode,CourseName,CourseTeacher,Department,Credit,Year,IsLab")]Course course)
        {
            await _courseBLL.AddCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var target = await _courseBLL.FindAsync(Id);
            if (target == null)
                return NotFound();
            return View(target);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int Id)
        {
            await _courseBLL.DeleteAsync(Id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var target = await _courseBLL.FindAsync(Id);
            if(target==null)
                return NotFound();
            ViewBag.Departments = makeDropdownList("department");
            ViewBag.Teachers = makeDropdownList("teacher");
            return View(target);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Update(int Id, [Bind("CourseCode,CourseName,CourseTeacher,Department,Credit,Year")]Course newCourse)
        {
            await _courseBLL.UpdateAsync(Id, newCourse);
            return RedirectToAction(nameof(Index));
        }
    }
}
