using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class CourseBLL:ICourseBLL
    {
        private readonly CourseRepository _courseRepository;
        public CourseBLL(CourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public List<Course> GetCourseList()
        {
            return _courseRepository.GetAll();
        }

        public List<DropdownItem> GetDropdownList(string contentName)
        {
            return _courseRepository.DropdownList(contentName);
        }

        public async Task AddCourseAsync(Course course)
        {
            _courseRepository.Add(course);
            await _courseRepository.SaveChangesAsync();
        }

        public async Task<Course?> FindAsync(int id)
        {
            return await _courseRepository.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var target =await _courseRepository.FindAsync(id);
            if (target is null)
                return;
            _courseRepository.Remove(target);
            await _courseRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, Course course)
        {
            var target= await _courseRepository.FindAsync(id);
            if (target is null) 
                return;
            target.CourseCode=course.CourseCode;
            target.CourseName=course.CourseName;
            target.CourseTeacher=course.CourseTeacher;
            target.Department=course.Department;
            target.Credit=course.Credit;
            target.Year=course.Year;
            target.IsLab=course.IsLab;
            _courseRepository.Update(target);
            await _courseRepository.SaveChangesAsync();
        }
    }
}
