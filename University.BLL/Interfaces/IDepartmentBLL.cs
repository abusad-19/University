using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IDepartmentBLL
    {
        List<Department> GetDepartmentList();
        Task AddAsync(Department department);
        Task<Department?> FindAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, Department department);
    }
}
