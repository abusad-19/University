namespace University.DAL.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Writer { get; set; }
        public string? Publication { get; set; }
        public string? Description { get; set; }
        public bool isBooked { get; set; } = false; 
    }
}
