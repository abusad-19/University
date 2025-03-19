using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IUserBLL
    {
        List<User> GetAll();
        string AddUser(User user);
        User? GetUserById(int id);
        void DeleteUser(User user);
        void UpdateUser(User oldUser, User newUser);
        Role? GetRoleById(int id);
        void AddUserRole(int userId, int roleId);
        (List<Role>?, List<Role>) GiveRole(int userId);
        void RemoveUserRole(int userId, int roleId);
        (List<Permission>, List<Role>) GetPermissionsAndRolesOfSingleUser(int userId);
    }
}
