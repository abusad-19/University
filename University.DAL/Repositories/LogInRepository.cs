using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class LogInRepository
    {
        private readonly appDBcontext _context;
        public LogInRepository(appDBcontext context)
        {
            _context = context;
        }

        public User? GetUser(int userCode)
        {
            return _context.UserTable.FirstOrDefault(u => u.UserCode == userCode);
        }
    }
}
