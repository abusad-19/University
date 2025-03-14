using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class PermissionRepository
    {
        private readonly appDBcontext _context;
        public PermissionRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<Permission> GetPermissions()
        {
            return _context.PermissionTable.ToList();
        }
        public void AddPermission(Permission permission)
        {
            _context.PermissionTable.Add(permission);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public Permission? GetPermissionById(int id)
        {
            return _context.PermissionTable.Find(id);
        }

        public void Remove(Permission permission)
        {
            _context.PermissionTable.Remove(permission);
        }
        public void UpdatePermission(Permission permission)
        {
            _context.PermissionTable.Update(permission);
        }
    }
}
