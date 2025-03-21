using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.BLL.Interfaces;
using University.DAL.Models;


namespace University.MVC.Controllers
{
    [Authorize(Policy = "CanTeacher_CRUD_operation")]
    public class TeacherController : Controller
    {
        private readonly ITeacherBLL _teacherBLL;
        public TeacherController(ITeacherBLL bll) 
        { 
            _teacherBLL = bll;
        }

        //[Authorize(Policy = "CanTeacher_CRUD_operation")]
        public IActionResult Index()
        {
            return View(_teacherBLL.GetTeacherList());
        }

        //making dropdown list of department
        public List<SelectListItem> makeDropdownListOfDepartment()
        {
            List<SelectListItem> departments= new List<SelectListItem>();
            var depts=_teacherBLL.GetTeacherDropdownList();
            foreach(var element in depts)
            { 
                departments.Add(new SelectListItem { Text=element.Text, Value=element.Value });
            }
            return departments;
        }

        //[Authorize(Policy = "CanTeacher_CRUD_operation")]
        public IActionResult Create()
        {
            ViewBag.Departments=makeDropdownListOfDepartment();
            return View();
        }

        //[Authorize(Policy = "CanTeacher_CRUD_operation")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("TeacherId,TeacherName,Department")]Teacher teacher)
        {
            await _teacherBLL.AddAsync(teacher);
            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Policy = "CanTeacher_CRUD_operation")]
        public async Task<IActionResult> Delete(int id)
        {
            if(id is 0)
                return NotFound();
            var teacher=await _teacherBLL.FindAsync(id);
            if (teacher == null)
                return NotFound();
            return View(teacher);
        }

        //[Authorize(Policy = "CanTeacher_CRUD_operation")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            if (id is 0)
                return NotFound();
            await _teacherBLL.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        //[Authorize(Policy = "CanTeacher_CRUD_operation")]
        public async Task<IActionResult> Edit(int id)
        {
            if(id is 0)
                return NotFound();
            var tutor = await _teacherBLL.FindAsync(id);
            if (tutor == null)
                return NotFound();
            ViewBag.Departments = makeDropdownListOfDepartment();
            return View(tutor);
        }

        //[Authorize(Policy = "CanTeacher_CRUD_operation")]
        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> Update(int id, [Bind("TeacherId,TeacherName,Department")]Teacher newTutor)
        {
            if(id is 0)
                return NotFound();
            await _teacherBLL.UpdateAsync(id, newTutor);
            return RedirectToAction(nameof(Index));
        }
    }
}
