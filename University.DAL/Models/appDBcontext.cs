using Microsoft.EntityFrameworkCore;
namespace University.DAL.Models
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext>options) 
            :base(options) 
        { 
        }

        public DbSet<Student> StudentTable { get; set; }
        public DbSet<Teacher> TeacherTable { get; set;}
        public DbSet<Department> DepartmentTable { get; set; }
        public DbSet<Course> CourseTable { get; set;}

        public DbSet<StudentResult> StudentResultTable { get; set;}
        public DbSet<StudentResultForYear> StudentResultForYearTable { get;set; }
        public DbSet<Book> BookTable { get; set; }
        public DbSet<Cart> CartTable { get; set; }
        public DbSet<LendBook> LendBookTable { get; set;}
    }
}
