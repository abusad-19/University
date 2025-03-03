using Microsoft.AspNetCore.Mvc;
using University.BLL.Services;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentBLL _departmentBLL;
        public DepartmentController(DepartmentBLL bll)
        {
            _departmentBLL = bll;
        }
        public IActionResult Index()
        {
            return View(_departmentBLL.GetDepartmentList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("DepartmentCode,DepartmentName")]Department department)
        {
            await _departmentBLL.AddAsync(department);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id == 0)
                return NotFound();
            var department = await _departmentBLL.FindAsync(id);
            if(department == null)
                return NotFound();
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            await _departmentBLL.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id == 0)
                return NotFound();
            var department = await _departmentBLL.FindAsync(id);
            if(department == null)
                return NotFound();
            return View(department);
        }

        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> Update(int id, [Bind("DepartmentCode,DepartmentName")]Department department)
        {
            await _departmentBLL.UpdateAsync(id, department);
            return RedirectToAction(nameof(Index));
        }
    }
}
