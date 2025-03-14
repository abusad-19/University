using Microsoft.EntityFrameworkCore;
using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class UserBLL : IUserBLL
    {
        private readonly UserRepository _repository;
        public UserBLL(UserRepository repository)
        {
            _repository = repository;
        }

        public List<User> GetAll()
        {
            return _repository.GetUsers();
        }
        public void AddUser(User user)
        {
            _repository.AddUser(user);
            _repository.SaveChanges();
        }

        public User? GetUserById(int id)
        {
            return _repository.GetUserById(id);
        }

        public void DeleteUser(User user)
        {
            _repository.DeleteUser(user);
            _repository.SaveChanges();
        }
        public void UpdateUser(User oldUser, User newUser)
        {
            oldUser.UserEmail= newUser.UserEmail;
            oldUser.Password= newUser.Password;
            oldUser.UserType= newUser.UserType;

            _repository.UpdateUser(oldUser);
            _repository.SaveChanges();
        }

        public Permission? GetPermissionById(int id)
        {
            return _repository.GetPermissionById(id);
        }

        public void AddUserPermissions(int userId,int permissionId)
        {
            UserPermissions temp = new UserPermissions();
            temp.UserId = userId;
            temp.PermissionId = permissionId;
            _repository.AddUserPermissions(temp);
            _repository.SaveChanges();
        }

        
        public (List<Permission>?, List<Permission>) GiveAccess(int userId)
        {
            var permissions = _repository.GetAllPermissions();
            var userPermission = _repository.GetASingleUserPermissions(userId);
            if (userPermission.Count is 0)
                return (null,permissions);

            List<Permission> toAdd = new List<Permission>();
            List<Permission> added = new List<Permission>();
            foreach (var item1 in permissions)
            {
                bool Isadded = false;
                foreach(var item2 in userPermission)
                {
                    if(item1.Id == item2.PermissionId)
                    {
                        Isadded = true;
                        break;
                    }
                }

                if(Isadded is true)
                {
                    added.Add(item1);
                }
                else
                {
                    toAdd.Add(item1);
                }
            }
            return (added, toAdd);
        }

        public void RemoveUserPermission(int userId,int permitId)
        {
            var target = _repository.GetUserPermissionByUserId_PermissionId(userId, permitId);
            if (target != null)
            {
                _repository.RemoveUserPermission(target);
                _repository.SaveChanges();
            }
        }
    }
}
