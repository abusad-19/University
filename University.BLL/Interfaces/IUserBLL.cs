using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IUserBLL
    {
        List<User> GetAll();
        void AddUser(User user);
        User? GetUserById(int id);
        void DeleteUser(User user);
        void UpdateUser(User oldUser, User newUser);
        Permission? GetPermissionById(int id);
        void AddUserPermissions(int userId, int permissionId);
        (List<Permission>?, List<Permission>) GiveAccess(int userId);
        void RemoveUserPermission(int userId, int permitId);
    }
}
