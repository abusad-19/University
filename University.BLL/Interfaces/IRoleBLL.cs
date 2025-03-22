using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IRoleBLL
    {
        List<Role> GetAllRole();
        void AddRole(Role role);
        Role? GetRoleById(int id);
        void RemoveRole(Role role);
        void UpdateRole(Role oldRole, Role newRole);
        (List<Permission>?, List<Permission>) GiveAccess(int roleId);
        Permission? GetPermissionById(int id);
        void AddRolePermissions(int roleId, int permissionId);
        void RemoveRolePermission(int roleId, int permitId);
    }
}
