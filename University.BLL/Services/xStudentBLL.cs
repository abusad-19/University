using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class xStudentBLL
    {
        private readonly xStudentRepository _repository;
        public xStudentBLL(xStudentRepository repository)
        {
            _repository = repository;
        }

        public List<LendBook> GetIssuedBooks(int studentId)
        {
            return _repository.GetIssuedBooks(studentId);
        }
    }
}
