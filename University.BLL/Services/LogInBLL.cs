using University.BLL.Interfaces;
using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class LogInBLL:ILogInBLL
    {
        private readonly LogInRepository _repository;
        public LogInBLL(LogInRepository repository)
        {
            _repository = repository;
        }

        public User? GetUser(int userCode)
        {
            return _repository.GetUser(userCode);
        }

        public Department? GetDepartment(string departmentName)
        {
            return _repository.GetDepartment(departmentName);
        }
    }
}
