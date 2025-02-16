using System.ComponentModel.DataAnnotations;

namespace University.DAL.Models
{
    public class StudentResult
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public int CourseCredit { get; set; }
        public int? Mark { get; set; }
        public float? GPA { get; set; }
        public string Year { get; set; } 
    }
}
