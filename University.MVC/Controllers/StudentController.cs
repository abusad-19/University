using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.DAL.Models;


namespace University.MVC.Controllers
{
    public class StudentController : Controller
    {
        public readonly appDBcontext _context;
        public StudentController(appDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.StudentTable.ToList());
        }

        //making dropdown list for department
        public List<SelectListItem> makeListOfDepartmentForDropdown()
        {
            List<SelectListItem> departments=new List<SelectListItem>();
            foreach(var element in _context.DepartmentTable)
            {
                departments.Add(new SelectListItem { Text = element.DepartmentName, Value = element.DepartmentName });
            }
            return departments;
        }
  
        public IActionResult Create()
        {
            ViewBag.Departments = makeListOfDepartmentForDropdown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("StudentId,StudentName,Department,Session")]Student pupil) 
        {
            _context.StudentTable.Add(pupil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id) 
        { 
            if(id == 0)
                return NotFound();
            var pupil=await _context.StudentTable.FirstOrDefaultAsync(x => x.Id == id);
            if(pupil == null)
                return NotFound();
            return View(pupil);  
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var pupil = await _context.StudentTable.FindAsync(id);
            if(pupil == null)
                return NotFound();
            _context.StudentTable.Remove(pupil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pupil = await _context.StudentTable.FirstOrDefaultAsync(x => x.Id == id);
            if(pupil == null)
                return NotFound();
            ViewBag.Departments = makeListOfDepartmentForDropdown();
            return View(pupil);
        }

        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> Update(int id,[Bind("StudentId,StudentName,Department,Session")]Student pupil)
        {
            var oldPupil = await _context.StudentTable.FindAsync(id);
            if (oldPupil == null)
                return NotFound();
            oldPupil.StudentId = pupil.StudentId;
            oldPupil.StudentName = pupil.StudentName;
            oldPupil.Department = pupil.Department;
            oldPupil.Session = pupil.Session;
            _context.StudentTable.Update(oldPupil);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
