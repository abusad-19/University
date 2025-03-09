using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class DepartmentBLL:IDepartmentBLL
    {
        private readonly DepartmentRepository _repository;
        public DepartmentBLL(DepartmentRepository repository)
        {
            _repository = repository;
        }

        public List<Department> GetDepartmentList()
        {
            return _repository.GetAll();
        }

        public async Task AddAsync(Department department)
        {
            _repository.Add(department);
            await _repository.SaveChangesAsync();
        }
        
        public async Task<Department?> FindAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            if (id == 0)
                return;
            var target= await _repository.FindAsync(id);
            if(target == null)
                return;
            _repository.Remove(target);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id,  Department department)
        {
            if(id== 0)
                return;
            var target=await _repository.FindAsync(id);
            if (target == null)
                return;
            target.DepartmentCode = department.DepartmentCode;
            target.DepartmentName = department.DepartmentName;
            _repository.Update(target);
            await _repository.SaveChangesAsync();
        }
    }
}
