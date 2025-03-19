using University.DAL.Models;

namespace University.MVC.View_Models
{
    public class LogInViewModel
    {
        public List<Permission> Permissions { get; set; }
        public int UserId { get; set; }
    }
}
