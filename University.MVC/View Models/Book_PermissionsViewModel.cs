using University.DAL.Models;

namespace University.MVC.View_Models
{
    public class Book_PermissionsViewModel
    {
        public List<Book> Books { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
