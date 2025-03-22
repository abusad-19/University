using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class StudentBLL: IStudentBLL
    {
        private readonly StudentRepository _studentRepository;
        public StudentBLL(
            StudentRepository studentRepository) 
        {
            _studentRepository = studentRepository;
        }

        public async Task UpdateAsync(int id, Student pupil)
        {
            var oldPupil = await _studentRepository.FindAsync(id);
            if (oldPupil == null)
                return;
            oldPupil.StudentId = pupil.StudentId;
            oldPupil.StudentName = pupil.StudentName;
            oldPupil.Department = pupil.Department;
            oldPupil.Session = pupil.Session;
            _studentRepository.Update(oldPupil);
            await _studentRepository.SaveChangesAsync();           
        }

        public List<Student> MakeStudentToList()
        {
            return _studentRepository.StudentToList();
        }
        
        public List<DropdownItem> DropdownItemForDepartment() 
        {
            return _studentRepository.DepartmentDropdownItem();
        }

        public void AddStudent(Student pupil)
        {
            _studentRepository.Add(pupil);
        }

        public async  Task<Student> FindStudentAsync(int id) //find student by his Id
        {
            return await _studentRepository.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var target = await _studentRepository.FindAsync(id);
            if (target == null)
                return;
            _studentRepository.Remove(target);
            _studentRepository.SaveChangesAsync();
        }
    }
}
