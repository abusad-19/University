using Microsoft.AspNetCore.Mvc;
using University.BLL.Interfaces;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserBLL _userBll;
        public UserController(IUserBLL userBll)
        {
            _userBll = userBll;
        }

        public IActionResult Index()
        {
            return View(_userBll.GetAll());
        }

        public IActionResult Create(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if(user.UserCode <=0 || user.Password==null)
                return RedirectToAction(nameof(Create),
                    new { errorMessage ="Please enter UserCode & Password"});

            var status=_userBll.AddUser(user);
            if(status== "undone")
                return RedirectToAction(nameof(Create),
                    new { errorMessage = "This UserCode is already used. Please use another email" });

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return NotFound();
            var target=_userBll.GetUserById(id);
            if (target is null)
                return NotFound();
            return View(target);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            if(id<=0) return NotFound();

            var target = _userBll.GetUserById(id);
            if (target is null)
                return NotFound();
            
            _userBll.DeleteUser(target);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0)
                return NotFound();
            var target =_userBll.GetUserById(id);
            if(target is null)
                return NotFound();
            return View(target);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult Update(int id, [Bind("UserCode,Password,UserType")] User user)
        {
            if(id<=0 || user is null)
                return NotFound();

            var target = _userBll.GetUserById(id);
            if (target is null)
                return NotFound();

            _userBll.UpdateUser(target,user);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GiveRole(int id)//user id
        {
            if(id<=0)
                return NotFound();
            var target = _userBll.GetUserById(id);
            if (target is null)
                return NotFound();

            ViewBag.userId=id;
            var temp=_userBll.GiveRole(id);
            if(temp.Item1  is null)
                return View(temp.Item2);

            ViewBag.added = temp.Item1;
            return View(temp.Item2);
        }

        public IActionResult GivePermit(int userId,int roleId)
        {
            if(userId<=0 || roleId<=0)
                return NotFound();
            var user= _userBll.GetUserById(userId);
            var permission=_userBll.GetRoleById(roleId);
            if(user is null || permission is null)
                return NotFound();

            _userBll.AddUserRole(userId,roleId);
            return RedirectToAction(nameof(GiveRole), new {id=userId});
        }

        public IActionResult RemovePermit(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
                return NotFound();
            _userBll.RemoveUserRole(userId,roleId);
            return RedirectToAction(nameof(GiveRole), new {id=userId});
        }

        public IActionResult ShowRolesAndPermissions(int id)//userId
        {
            if(id<=0)
                return NotFound();

            var user=_userBll.GetUserById(id);
            if (user is null)
                return NotFound();
            var temp=_userBll.GetPermissionsAndRolesOfSingleUser(id);
            ViewBag.Permissions=temp.Item1;
            ViewBag.Roles=temp.Item2;
            return View();
        }


    }
}
