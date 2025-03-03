using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using University.BLL.Services;
using University.DAL.Models;


namespace University.MVC.Controllers
{
    public class TeacherController : Controller
    {
        private readonly TeacherBLL _teacherBLL;
        public TeacherController(TeacherBLL bll) 
        { 
            _teacherBLL = bll;
        }
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

        public IActionResult Create()
        {
            ViewBag.Departments=makeDropdownListOfDepartment();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("TeacherId,TeacherName,Department")]Teacher teacher)
        {
            await _teacherBLL.AddAsync(teacher);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id is 0)
                return NotFound();
            var teacher=await _teacherBLL.FindAsync(id);
            if (teacher == null)
                return NotFound();
            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            if (id is 0)
                return NotFound();
            await _teacherBLL.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

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
