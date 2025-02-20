namespace University.DAL.Models
{
    public class StudentResultForYear
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Year { get; set; }
        public float GPApoint { get; set; }
    }
}
