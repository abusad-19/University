using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.BLL.Interfaces;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleBLL _roleBLL;
        public RoleController(IRoleBLL roleBLL)
        {
            _roleBLL = roleBLL;
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Index()
        {
            return View(_roleBLL.GetAllRole());
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (role == null)
                return NotFound();
            _roleBLL.AddRole(role);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return NotFound();

            var target = _roleBLL.GetRoleById(id);
            if (target is null)
                return NotFound();
            return View(target);
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if (id <= 0) return NotFound();

            var target = _roleBLL.GetRoleById(id);
            if (target is null)
                return NotFound();

            _roleBLL.RemoveRole(target);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult Edit(int id)
        {
            if (id == 0)
                return NotFound();
            var target = _roleBLL.GetRoleById(id);
            if (target is null)
                return NotFound();
            return View(target);
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        [HttpPost, ActionName("Edit")]
        public IActionResult Update(int id, [Bind("RoleName")] Role role)
        {
            if (id <= 0 || role is null)
                return NotFound();

            var target = _roleBLL.GetRoleById(id);
            if (target is null)
                return NotFound();

            _roleBLL.UpdateRole(target, role);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult GiveAccess(int id)//Role id
        {
            if (id <= 0)
                return NotFound();
            var target = _roleBLL.GetRoleById(id);
            if (target is null)
                return NotFound();

            ViewBag.roleId = id;
            var temp = _roleBLL.GiveAccess(id);
            if (temp.Item1 is null)
                return View(temp.Item2);

            ViewBag.added = temp.Item1;
            return View(temp.Item2);
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult GivePermit(int roleId, int permissionId)
        {
            if (roleId <= 0 || permissionId <= 0)
                return NotFound();
            var role=_roleBLL.GetRoleById(roleId);
            var permission = _roleBLL.GetPermissionById(permissionId);
            if (role is null || permission is null)
                return NotFound();

            _roleBLL.AddRolePermissions(roleId,permissionId);
            return RedirectToAction(nameof(GiveAccess), new { id = roleId });
        }

        [Authorize(Policy = "CanManagePermissionRole")]
        public IActionResult RemovePermit(int roleId, int permissionId)
        {
            if (roleId <= 0 || permissionId <= 0)
                return NotFound();
            _roleBLL.RemoveRolePermission(roleId, permissionId);
            return RedirectToAction(nameof(GiveAccess), new { id = roleId });
        }
    }
}
