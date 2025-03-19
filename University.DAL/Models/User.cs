namespace University.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public int UserCode { get; set;}
        public string? Password { get; set;}
        public string? UserType { get; set;}
    }
}
