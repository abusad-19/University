using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class xDepartmentBLL
    {
        private readonly xDepartmentRepository _repository;
        public xDepartmentBLL(xDepartmentRepository repository)
        {
            _repository = repository;
        }

        public Department? FindDepartment(int departmentCode)
        {
            return _repository.FindDepartment(departmentCode);
        }

        public List<Teacher> GetAllTeachersByDepartment(string departmentName)
        {
            return _repository.GetAllTeachersByDepartment(departmentName);
        }

        public List<Course> GetAllCoursesByDepartment(string departmentName)
        {
            return _repository.GetAllCoursesByDepartment(departmentName);
        }

        public List<Student>? GetAllStudentsByDepartmentAndYear(string deptName, string year)
        {
            return _repository.GetAllStudentsByDepartmentAndYear(deptName, year);
        }

        public Student? GetStudentById(int studentId)
        {
            return _repository.GetStudentById(studentId);
        }

        public (string,bool,bool,bool,float?,List<StudentResult>)? GetAllCoursesForEachStudent(int studentId, string year)
        {
            if (studentId == 0 || year is null)
                return null;

            var student = _repository.GetStudentById(studentId);
            if (student == null)
                return null;
            
            var coursesOfYear=_repository.GetAllCoursesByDepartmentAndYear(student.Department, year);
            var enrolledCourses=_repository.GetAllEnrolledCourses(studentId, year);

            bool isAllEnrolled = false;
            bool isMarkNull = false;
            if (coursesOfYear.Count==enrolledCourses.Count)
            {
                isAllEnrolled = true;
                foreach (var item in enrolledCourses)
                {
                    if (item.Mark is null)
                    {
                        isMarkNull = true;
                        break;
                    }
                }
            }

            bool isNeedUpdate= false;
            var yearResult=_repository.GetYearResult(studentId, year);
            float? gpaPoint = null;
            if(yearResult!=null)
            {
                isNeedUpdate = true;
                gpaPoint=yearResult.GPApoint;
            }

            return (student.StudentName, isAllEnrolled, isMarkNull, isNeedUpdate,gpaPoint,enrolledCourses);
        }

        private float GPAPointCalculator(int mark)
        {
            if (mark >= 80)
                return 4.0F;
            else if (mark >= 75 && mark < 80)
                return 3.75F;
            else if (mark >= 70 && mark < 75)
                return 3.50F;
            else if (mark >= 65 && mark < 70)
                return 3.25F;
            else if (mark >= 60 && mark < 65)
                return 3.00F;
            else if (mark >= 55 && mark < 60)
                return 2.75F;
            else if (mark >= 50 && mark < 60)
                return 2.50F;
            else if (mark >= 45 && mark < 50)
                return 2.25F;
            else if (mark >= 40 && mark < 45)
                return 2.00F;
            else
                return 0;
        }

        public async Task SaveResultForSingleCourseAsync(StudentResult course)
        {
            var target = _repository.GetByStudentIdAndCourseId(course.StudentId, course.CourseCode);
            if (target is null) 
                return;

            target.Mark=course.Mark;
            if (target.CourseCredit == 2)
            {
                target.GPA = GPAPointCalculator(2 * course.Mark ?? 0);
            }
            else
            {
                target.GPA = GPAPointCalculator(course.Mark ?? 0);
            }

            _repository.UpdateStudentResult(target);
            await _repository.SaveChangesAsync();
        }

        public (string,List<StudentResult>,float)? GenerateYearFinalResult(int studentId)
        {
            var target=_repository.GetStudentById(studentId);
            if (target is null)
                return null;
            var courses = _repository.GetAllEnrolledCourses(studentId, target.Year);

            float up = 0;
            float down = 0;
            int lossCredit = 0;
            StudentResult Lab = new StudentResult();
            foreach (var course in courses)
            {
                up = up + course.CourseCredit * course.GPA ?? 0;
                down = down + course.CourseCredit;
                if (course.IsLab is true)
                    Lab = course;
                if (course.GPA == 0)
                    lossCredit = course.CourseCredit;
            }

            StudentResultForYear yearFinal = new StudentResultForYear();
            yearFinal.StudentId = studentId;
            yearFinal.Year = target.Year;
            if (lossCredit > 10 || Lab.GPA == 0)
            {
                yearFinal.GPApoint = 0;
            }
            else
            {
                yearFinal.GPApoint = up / down;
                if (target.Year == "First Year")
                    target.Year = "Second Year";
                else if (target.Year == "Second Year")
                    target.Year = "Third Year";
                else if (target.Year == "Third Year")
                    target.Year = "Fourth Year";
                else
                    target.Year = "Master's";
                _repository.UpdateStudent(target);
            }
            _repository.AddYearFinalResult(yearFinal);
            _repository.SaveChanges();

            return (yearFinal.Year, courses, yearFinal.GPApoint);
        }

        public async Task<bool> UpdateYearResultAsync(int studentId, string year)
        {
            var yearFinal = _repository.GetYearResult(studentId, year);
            var courses = _repository.GetAllEnrolledCourses(studentId, year);

            if (yearFinal?.GPApoint == 0)
            {
                var target = _repository.GetStudentById(studentId); 
                    
                float up = 0;
                float down = 0;
                StudentResult labCourse = new StudentResult();
                int lossCredit = 0;
                foreach (var course in courses)
                {
                    up = up + course.CourseCredit * course.GPA ?? 0;
                    down = down + course.CourseCredit;

                    if (course.IsLab is true)
                        labCourse = course;
                    if (course.GPA == 0)
                        lossCredit = lossCredit + course.CourseCredit;
                }

                if (labCourse.GPA != 0 && lossCredit < 10)
                {
                    yearFinal.GPApoint = up / down;
                    _repository.UpdateYearFinalResult(yearFinal);

                    if (target?.Year == "First Year")
                        target.Year = "Second Year";
                    else if (target?.Year == "Second Year")
                        target.Year = "Third Year";
                    else if (target?.Year == "Third Year")
                        target.Year = "Fourth Year";
                    else
                        target!.Year = "Master's";
                    _repository.UpdateStudent(target);
                }
            }
            else
            {
                float up = 0;
                float down = 0;
                StudentResult labCourse = new StudentResult();
                int lossCredit = 0;
                foreach (var course in courses)
                {
                    up = up + course.CourseCredit * course.GPA ?? 0;
                    down = down + course.CourseCredit;

                    if (course.IsLab is true)
                        labCourse = course;
                    if (course.GPA == 0)
                        lossCredit = lossCredit + course.CourseCredit;
                }

                if (labCourse.GPA == 0 || lossCredit > 10)
                {
                    yearFinal!.GPApoint = 0;

                    //finding the target student
                    Student? target = new Student();
                    if (year == "First Year")
                    {
                        var mayBeTarget =_repository.GetStudentById(studentId);
                            
                        if (mayBeTarget?.Year== "Second Year")
                        {
                            target = mayBeTarget;
                        }
                        else
                        {
                            target = null;
                        }
                    }
                    else if (year == "Second Year")
                    {
                        var mayBeTarget = _repository.GetStudentById(studentId);

                        if (mayBeTarget?.Year == "Third Year")
                        {
                            target = mayBeTarget;
                        }
                        else
                        {
                            target = null;
                        }
                    }
                    else if (year == "Third Year")
                    {
                        var mayBeTarget = _repository.GetStudentById(studentId);

                        if (mayBeTarget?.Year == "Fourth Year")
                        {
                            target = mayBeTarget;
                        }
                        else
                        {
                            target = null;
                        }
                    }
                    else
                    {
                        var mayBeTarget = _repository.GetStudentById(studentId);

                        if (mayBeTarget?.Year == "Master's")
                        {
                            target = mayBeTarget;
                        }
                        else
                        {
                            target = null;
                        }
                    }



                    if (target is null)
                    {
                        return true;
                    }


                    //give the target student Demossion
                    if (target.Year == "Second Year")
                        target.Year = "First Year";
                    else if (target.Year == "Third Year")
                        target.Year = "Second Year";
                    else if (target.Year == "Fourth Year")
                        target.Year = "Third Year";
                    else
                        target.Year = "Fourth Year";
                    _repository.UpdateStudent(target);
                }
                else
                {
                    yearFinal!.GPApoint = up / down;
                }
                _repository.UpdateYearFinalResult(yearFinal);
                
            }
            
            await _repository.SaveChangesAsync();
            return false;
        }


    }
}
