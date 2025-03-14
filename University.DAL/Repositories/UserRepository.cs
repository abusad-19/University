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

        public List<Role> GetAllRoles()
        {
            return _context.RoleTable.ToList();
        }

        public Role? GetRoleById(int id)
        {
            return _context.RoleTable.Find(id);
        }

        public void AddUserRole(UserRole temp)
        {
            _context.UserRoleTable.Add(temp);
        }

        public List<UserRole> GetASingleUserRoles(int userId)
        {
            return _context.UserRoleTable.Where(p=>p.UserId == userId).ToList();
        }
        public UserRole? GetUserRoleByUserId_RoleId(int userId, int roleId)
        {
            return _context.UserRoleTable.
                Where(u => u.UserId == userId && u.RoleId == roleId).
                FirstOrDefault();
        }

        public void RemoveUserPermission(UserRole target)
        {
            _context.UserRoleTable.Remove(target);
        }
    }
}
