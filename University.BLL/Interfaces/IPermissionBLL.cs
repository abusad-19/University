using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IPermissionBLL
    {
        List<Permission> GetAll();
        void AddPermission(Permission permission);
        Permission? GetPermissionById(int id);
        void DeletePermission(Permission permission);
        void UpdatePermission(Permission oldPermission, Permission newPermission);
    }
}
