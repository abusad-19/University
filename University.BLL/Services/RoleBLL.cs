using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class RoleBLL:IRoleBLL
    {
        private readonly RoleRepository _repository;
        public RoleBLL(RoleRepository repository)
        {
            _repository = repository;
        }

        public List<Role> GetAllRole()
        {
            return _repository.GetAllRole();
        }

        public void AddRole(Role role)
        {
            _repository.AddRole(role);
            _repository.SaveChanges();
        }

        public Role? GetRoleById(int id)
        {
            return _repository.GetRoleById(id);
        }
        public void RemoveRole(Role role)
        {
            _repository.RemoveRole(role);
            _repository.SaveChanges();
        }

        public void UpdateRole(Role oldRole,Role newRole)
        {
            oldRole.RoleName = newRole.RoleName;
            _repository.UpdateRole(oldRole);
            _repository.SaveChanges();
        }
        public (List<Permission>?, List<Permission>) GiveAccess(int roleId)
        {
            var permissions = _repository.GetAllPermissions();
            var rolePermission = _repository.GetASingleRolePermissions(roleId);
            if (rolePermission.Count is 0)
                return (null, permissions);

            List<Permission> toAdd = new List<Permission>();
            List<Permission> added = new List<Permission>();
            foreach (var item1 in permissions)
            {
                bool Isadded = false;
                foreach (var item2 in rolePermission)
                {
                    if (item1.Id == item2.PermissionId)
                    {
                        Isadded = true;
                        break;
                    }
                }

                if (Isadded is true)
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

        public Permission? GetPermissionById(int id)
        {
            return _repository.GetPermissionById(id);
        }

        public void AddRolePermissions(int roleId, int permissionId)
        {
            RolePermissions temp = new RolePermissions();
            temp.RoleId = roleId;
            temp.PermissionId = permissionId;
            _repository.AddRolePermissions(temp);
            _repository.SaveChanges();
        }

        public void RemoveRolePermission(int roleId, int permitId)
        {
            var target = _repository.GetRolePermissionByRoleId_PermissionId(roleId, permitId);
            if (target != null)
            {
                _repository.RemoveRolePermission(target);
                _repository.SaveChanges();
            }
        }
    }
}
