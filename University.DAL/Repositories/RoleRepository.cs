using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class RoleRepository
    {
        private readonly appDBcontext _context;
        public RoleRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<Role> GetAllRole()
        {
            return _context.RoleTable.ToList();
        }

        public void AddRole(Role role)
        {
            _context.RoleTable.Add(role);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Role? GetRoleById(int id)
        {
            return _context.RoleTable.Find(id);
        }

        public void RemoveRole(Role role)
        {
            _context.RoleTable.Remove(role);
        }

        public void UpdateRole(Role role)
        {
            _context.RoleTable.Update(role);
        }

        public List<Permission> GetAllPermissions()
        {
            return _context.PermissionTable.ToList();
        }

        public List<RolePermissions> GetASingleRolePermissions(int roleId)
        {
            return _context.RolePermissionsTable.Where(p => p.RoleId == roleId).ToList();
        }

        public Permission? GetPermissionById(int id)
        {
            return _context.PermissionTable.Find(id);
        }
        public void AddRolePermissions(RolePermissions temp)
        {
            _context.RolePermissionsTable.Add(temp);
        }

        public RolePermissions? GetRolePermissionByRoleId_PermissionId(int roleId, int permitId)
        {
            return _context.RolePermissionsTable.
                Where(u => u.RoleId == roleId && u.PermissionId == permitId).
                FirstOrDefault();
        }
        public void RemoveRolePermission(RolePermissions target)
        {
            _context.RolePermissionsTable.Remove(target);
        }
    }
}
