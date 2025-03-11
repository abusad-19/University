namespace University.DAL.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set;}
        public string? Department { get; set;}
        public string? Session { get; set; }
        public string Year { get; set; } ="First Year";
        public string? AccountPassword {  get; set; }
    }
}
