using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.BLL.Interfaces;
using University.DAL.Models;


namespace University.MVC.Controllers
{
    [Authorize(Policy = "CanStudent_CRUD_operation")]
    public class StudentController : Controller
    {
        private readonly IStudentBLL _studentBll;
        public StudentController(IStudentBLL studentBLL)
        {
            _studentBll = studentBLL;
        }

        public IActionResult Index()
        {
            return View(_studentBll.MakeStudentToList());
        }

        //making dropdown list for department
        public List<SelectListItem> makeListOfDepartmentForDropdown()
        {
            List<DropdownItem> list = _studentBll.DropdownItemForDepartment();
            List<SelectListItem> departments = new List<SelectListItem>();
            foreach (var element in list)
            {
                departments.Add(new SelectListItem { Text = element.Text, Value = element.Value });
            }
            return departments;
        }

        public IActionResult Create()
        {
            ViewBag.Departments = makeListOfDepartmentForDropdown();
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("StudentId,StudentName,Department,Session")]Student pupil) 
        {
            _studentBll.AddStudent(pupil);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id) 
        { 
            if(id == 0)
                return NotFound();
            var pupil = await _studentBll.FindStudentAsync(id);
            if (pupil == null)
                return NotFound();
            return View(pupil);  
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            await _studentBll.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pupil = await _studentBll.FindStudentAsync(id);
            if(pupil == null)
                return NotFound();
            ViewBag.Departments = makeListOfDepartmentForDropdown();
            return View(pupil);
        }

        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> Update(int id,[Bind("StudentId,StudentName,Department,Session")]Student pupil)
        {
            await _studentBll.UpdateAsync(id, pupil);
            return RedirectToAction(nameof(Index));
        }
    }
}
