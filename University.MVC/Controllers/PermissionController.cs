using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.BLL.Interfaces;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class PermissionController : Controller
    {
        private readonly IPermissionBLL _permissionBll;
        public PermissionController(IPermissionBLL permissionBll)
        {
            _permissionBll = permissionBll;
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Index()
        {
            return View(_permissionBll.GetAll());
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        [HttpPost]
        public IActionResult Create(Permission permission)
        {
            if (permission == null)
                return NotFound();
            _permissionBll.AddPermission(permission);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return NotFound();
            var target = _permissionBll.GetPermissionById(id);
            if (target is null)
                return NotFound();
            return View(target);
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if (id <= 0) return NotFound();

            var target = _permissionBll.GetPermissionById(id);
            if (target is null)
                return NotFound();

            _permissionBll.DeletePermission(target);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
                return NotFound();
            var target = _permissionBll.GetPermissionById(id);
            if (target is null)
                return NotFound();
            return View(target);
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        [HttpPost, ActionName("Edit")]
        public IActionResult Update(int id, [Bind("Name")] Permission permission)
        {
            if (id <= 0 || permission is null)
                return NotFound();

            var target = _permissionBll.GetPermissionById(id);
            if (target is null)
                return NotFound();

            _permissionBll.UpdatePermission(target, permission);
            return RedirectToAction(nameof(Index));
        }
    }
}
