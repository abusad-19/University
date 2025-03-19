using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface ILogInBLL
    {
        User? GetUser(int userCode);
    }
}
