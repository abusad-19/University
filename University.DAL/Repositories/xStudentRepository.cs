using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class xStudentRepository
    {
        private readonly appDBcontext _context;
        public xStudentRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<LendBook> GetIssuedBooks(int studentId)
        {
            return _context.LendBookTable.Where(b => b.StudentId == studentId).ToList();
        }
    }
}
