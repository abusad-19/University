using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using University.BLL.Interfaces;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class LogInController : Controller
    {
        private readonly ILogInBLL _loginBLL;
        private readonly IUserBLL _userBLL;
        public LogInController(ILogInBLL loginBLL,
            IUserBLL userBLL)
        {
            _loginBLL = loginBLL;
            _userBLL = userBLL;
        }

        public IActionResult Index(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View();
        }

        public async Task<IActionResult> LogIn(User user)
        {
            if(user.UserCode<=0 || user.Password is null)
                return RedirectToAction(nameof(Index),
                    new { errorMessage = "Please Enter Email And Password" });

            var consumer=_loginBLL.GetUser(user.UserCode);
            if(consumer is null)
                return RedirectToAction(nameof(Index),
                    new { errorMessage = "This email has no account" });

            if(user.Password!= consumer.Password)
            {
                return RedirectToAction(nameof(Index),
                    new { errorMessage = "Please enter correct password" });
            }

            var temp = _userBLL.GetPermissionsAndRolesOfSingleUser(consumer.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,consumer.UserType),
                new Claim("UserCode",$"{consumer.UserCode}")
            };

            foreach (var item in temp.Item1)
            {
                claims.Add(new Claim("Permission", $"{item.Name}"));
            }

            var claimsIdentity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true// Keep cookie after closing the browser
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
                );

            return RedirectToAction(nameof(UserDashboard));
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult UserDashboard()
        {
            return View();
        }
    }
}
