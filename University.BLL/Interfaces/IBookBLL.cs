using University.DAL.Models;

namespace University.BLL.Interfaces
{
    public interface IBookBLL
    {
        List<Book> GetAllBook();
        Task AddBookAsync(Book book);
        Book? GetBookById(int id);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        void AddCart(int bookId);
        bool RemoveCart(int bookId);
        List<Cart> GetCart();
        Student? GetStudentById(int studentId);
        void LendBook(Student student, List<Cart> cart);
        List<LendBook>? GetLendBooks(int studentId);
        int? ReturnLendBook(int lendBookId);
        public List<Book> CheckStock(string name, string writer);
    }
}
