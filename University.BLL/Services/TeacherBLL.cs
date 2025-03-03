using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class TeacherBLL
    {
        private readonly TeacherRepository _repository;
        public TeacherBLL(TeacherRepository repository)
        {
            _repository = repository;
        }

        public List<Teacher> GetTeacherList()
        {
            return _repository.GetAllTeacherList();
        }

        public List<DropdownItem> GetTeacherDropdownList()
        {
            return _repository.GetTeacherDropdownList();
        }

        public async Task AddAsync(Teacher teacher)
        {
            _repository.Add(teacher);
            await _repository.SaveChangesAsync();
        }

        public async Task<Teacher?> FindAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var target = await _repository.FindAsync(id);
            if (target == null)
                return;
            _repository.Remove(target);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Teacher teacher)
        {
            var target= await _repository.FindAsync(id);
            if (target == null) 
                return;
            target.TeacherName = teacher.TeacherName;
            target.TeacherId= teacher.TeacherId;
            target.Department= teacher.Department;
            
            _repository.Update(target);
            await _repository.SaveChangesAsync();
        }
    }
}
