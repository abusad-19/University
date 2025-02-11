using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class DepartmentController : Controller
    {
        public readonly appDBcontext _context;
        public DepartmentController(appDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.DepartmentTable.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("DepartmentCode,DepartmentName")]Department department)
        {
            _context.DepartmentTable.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == 0)
                return NotFound();
            var department=await _context.DepartmentTable.FirstOrDefaultAsync(x => x.Id == id); 
            if(department == null)
                return NotFound();
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var department=await _context.DepartmentTable.FindAsync(id);
            if(department==null)
                return NotFound();
            _context.DepartmentTable.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == 0)
                return NotFound();
            var department = await _context.DepartmentTable.FirstOrDefaultAsync(x => x.Id == id);
            if(department == null)
                return NotFound();
            return View(department);
        }

        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> Update(int id, [Bind("DepartmentCode,DepartmentName")]Department department)
        {
            var oldDepartment = await _context.DepartmentTable.FindAsync(id);
            if(oldDepartment == null)
                return NotFound();
            oldDepartment.DepartmentCode = department.DepartmentCode;
            oldDepartment.DepartmentName = department.DepartmentName;
            _context.DepartmentTable.Update(oldDepartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
