namespace University.DAL.Models
{
    public class LendBook
    {
        public int Id { get; set; }
        public int StudentId {  get; set; }
        public string? StudentName { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set;}
        public string? Writer { get; set;}
        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
