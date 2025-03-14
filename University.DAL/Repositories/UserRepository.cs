using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class UserRepository
    {
        private readonly appDBcontext _context;
        public UserRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<User> GetUsers()
        {
            return _context.UserTable.ToList();
        }
        public void AddUser(User user)
        {
            _context.UserTable.Add(user);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public User? GetUserById(int id)
        {
            return _context.UserTable.Find(id);
        }

        public void DeleteUser(User user)
        {
            _context.UserTable.Remove(user);
        }
        public void UpdateUser(User user)
        {
            _context.UserTable.Update(user);
        }

        public List<Permission> GetAllPermissions()
        {
            return _context.PermissionTable.ToList();
        }

        public Permission? GetPermissionById(int id)
        {
            return _context.PermissionTable.Find(id);
        }

        public void AddUserPermissions(UserPermissions temp)
        {
            _context.UserPermissionsTable.Add(temp);
        }

        public List<UserPermissions> GetASingleUserPermissions(int userId)
        {
            return _context.UserPermissionsTable.Where(p=>p.UserId == userId).ToList();
        }
        public UserPermissions? GetUserPermissionByUserId_PermissionId(int userId, int permitid)
        {
            return _context.UserPermissionsTable.
                Where(u => u.UserId == userId && u.PermissionId == permitid).
                FirstOrDefault();
        }

        public void RemoveUserPermission(UserPermissions target)
        {
            _context.UserPermissionsTable.Remove(target);
        }
    }
}
