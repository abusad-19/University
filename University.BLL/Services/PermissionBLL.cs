using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class PermissionBLL : IPermissionBLL
    {
        private readonly PermissionRepository _repository;
        public PermissionBLL(PermissionRepository repository)
        {
            _repository = repository;
        }

        public List<Permission> GetAll()
        {
            return _repository.GetPermissions();
        }
        public void AddPermission(Permission permission)
        {
            _repository.AddPermission(permission);
            _repository.SaveChanges();
        }

        public Permission? GetPermissionById(int id)
        {
            return _repository.GetPermissionById(id);
        }

        public void DeletePermission(Permission permission)
        {
            _repository.Remove(permission);
            _repository.SaveChanges();
        }
        public void UpdatePermission(Permission oldPermission, Permission newPermission)
        {
            oldPermission.Name = newPermission.Name;

            _repository.UpdatePermission(oldPermission);
            _repository.SaveChanges();
        }
    }
}
