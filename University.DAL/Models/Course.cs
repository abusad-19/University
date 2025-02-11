namespace University.DAL.Models
{
    public class Course
    {
        public int Id { get; set; }
        public int CourseCode { get; set; }
        public string? CourseName { get; set; }
        public string? CourseTeacher { get; set; }
        public string? Department { get; set; }
    }
}
