using University.DAL.Models;
using University.DAL.Repositories;

namespace University.BLL.Services
{
    public class BookBLL
    {
        private readonly BookRepository _repository;
        public BookBLL(BookRepository repository)
        {
            _repository = repository;
        }

        public List<Book> GetAllBook()
        {
            return _repository.GetAllBooks();
        }

        public async Task AddBookAsync(Book book)
        {
            _repository.AddBook(book);
            await _repository.SaveChangesAsync();
        }

        public Book? GetBookById(int id)
        {
            return _repository.GetBookById(id);
        }

        public async Task UpdateAsync(Book book)
        {
            _repository.Update(book);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var target=_repository.GetBookById(id);
            if(target != null)
            {
                _repository.Remove(target);
            }
            await _repository.SaveChangesAsync();
        }

        public void AddCart(int bookId)
        {
            Cart cart = new Cart();
            var book=_repository.GetBookById(bookId);
            cart.BookId = bookId;
            cart.BookName = book.Name;
            _repository.AddCart(cart);
        }

        public bool RemoveCart(int bookId)
        {
            var target= _repository.GetCartItemByBookId(bookId);
            if(target is null)
                return false;
            _repository.RemoveCart(target);
            return true;
        }

        public List<Cart> GetCart()
        {
            return _repository.GetCart();
        }

        public Student? GetStudentById(int studentId)
        {
            return _repository.GetStudentById(studentId);
        }

        public void LendBook(Student student, List<Cart> cart)
        {
            foreach(var item in cart)
            {
                LendBook temp=new LendBook();
                temp.StudentId = student.StudentId;
                temp.StudentName=student.StudentName;
                temp.BookId=item.BookId;
                temp.BookName=item.BookName;
                var book=_repository.GetBookById(item.BookId);
                temp.Writer = book.Writer;
                temp.IssueDate=DateTime.Now;
                _repository.AddLendBook(temp);
                _repository.RemoveCart(item);
            }
        }

        public List<LendBook>? GetLendBooks(int studentId)
        {
            return _repository.GetLendBooks(studentId);
        }

        public int? ReturnLendBook(int lendBookId)
        {
            var book=_repository.GetLendBookById(lendBookId);
            if (book == null)
                return null;
            book.ReturnDate = DateTime.Now;
            _repository.UpdateLendBook(book);
            _repository.SaveChanges();
            return book.StudentId;
        }

       
    }
}
