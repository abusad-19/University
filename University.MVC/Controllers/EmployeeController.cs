using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.BLL.Interfaces;
using University.DAL.Models;
using University.MVC.View_Models;

namespace University.MVC.Controllers
{
    [Authorize(Policy = "CanEmployee_CRUD_operation")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeBLL _employeeBll;
        public EmployeeController(IEmployeeBLL employeeBLL)
        {
            _employeeBll = employeeBLL;
        }
        public IActionResult Index()
        {
            EmployeeList_ViewModel Model = new EmployeeList_ViewModel();
            Model.Employees = _employeeBll.GetAll();
            return View(Model);
        }

        public IActionResult Create()
        {
            ViewBag.names=_employeeBll.GetAllDepartmentName();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if(employee == null) 
                return NotFound();

            _employeeBll.AddEmployee(employee);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
                return NotFound();
            var employee=_employeeBll.GetEmployeeById(id);
            ViewBag.names = _employeeBll.GetAllDepartmentName();
            return View(employee);
        }

        public IActionResult Update(Employee employee)
        {
            if(employee == null)
                return NotFound();

            _employeeBll.UpdateEmployee(employee);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return NotFound();
            var employee = _employeeBll.GetEmployeeById(id);
            return View(employee);
        }

        public IActionResult ConfirmDelete(int id)
        {
            if(id<=0)
                return NotFound();

            var employee = _employeeBll.GetEmployeeById(id);
            if(employee== null)
                return NotFound();

            _employeeBll.DeleteEmployee(employee);
            return RedirectToAction("Index");
        }
    }
}
