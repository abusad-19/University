using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using University.MVC.Models;


namespace University.MVC.Controllers
{
    public class TeacherController : Controller
    {
        public readonly appDBcontext _context;
        public TeacherController(appDBcontext context) 
        { 
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.TeacherTable.ToList());
        }

        //making dropdown list of department
        public List<SelectListItem> makeDropdownListOfDepartment()
        {
            List<SelectListItem> departments= new List<SelectListItem>();
            foreach(var element in _context.DepartmentTable)
            { 
                departments.Add(new SelectListItem { Text=element.DepartmentName, Value=element.DepartmentName });
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
            _context.TeacherTable.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
                return NotFound();
            var teacher=await _context.TeacherTable.FirstOrDefaultAsync(x => x.Id == id);
            if (teacher == null)
                return NotFound();
            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.TeacherTable.FindAsync(id);
            if (teacher == null)
                return NotFound();
            _context.TeacherTable.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if(id==null)
                return NotFound();
            var tutor=await _context.TeacherTable.FirstOrDefaultAsync(x=>x.Id == id);
            if (tutor == null)
                return NotFound();
            ViewBag.Departments = makeDropdownListOfDepartment();
            return View(tutor);
        }

        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> Update(int id, [Bind("TeacherId,TeacherName,Department")]Teacher newTutor)
        {
            var oldTutor = await _context.TeacherTable.FindAsync(id);
            if (oldTutor == null)
                return NotFound();

            oldTutor.TeacherId=newTutor.TeacherId;
            oldTutor.TeacherName = newTutor.TeacherName;
            oldTutor.Department = newTutor.Department;

            _context.TeacherTable.Update(oldTutor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
