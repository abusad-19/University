using Microsoft.EntityFrameworkCore;
using University.DAL.Models;

namespace University.DAL.Repositories
{
    public class BookRepository
    {
        private readonly appDBcontext _context;
        public BookRepository(appDBcontext context)
        {
            _context = context;
        }

        public List<Book> GetAllBooks()
        {
            return _context.BookTable.ToList();
        }

        public void AddBook(Book book)
        {
            _context.BookTable.Add(book);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Book? GetBookById(int id)
        {
            return _context.BookTable.Find(id);
        }

        public void Update(Book book)
        {
            _context.BookTable.Update(book);
        }

        public void Remove(Book book)
        {
            _context.BookTable.Remove(book);
        }

        public void AddCart(Cart cart)
        {
            _context.CartTable.Add(cart);
            _context.SaveChanges();
        }

        public void RemoveCart(Cart cart)
        {
            _context.CartTable.Remove(cart);
            _context.SaveChanges();
        }

        public Cart? GetCartItemByBookId(int bookId)
        {
            return _context.CartTable.FirstOrDefault(c=>c.BookId==bookId);
        }

        public List<Cart> GetCart()
        {
            return _context.CartTable.ToList();
        }

        public Student? GetStudentById(int studentId)
        {
            return _context.StudentTable.FirstOrDefault(s => s.StudentId == studentId);
        }

        public void AddLendBook(LendBook lend)
        {
            _context.LendBookTable.Add(lend);
            _context.SaveChanges();
        }

        public List<LendBook>? GetLendBooks(int studentId)
        {
            return _context.LendBookTable.FromSqlInterpolated
                ($"select * from LendBookTable where StudentId={studentId} and ReturnDate is null;")
                .ToList();
        }
        public void SaveChanges()
        {
            _context.SaveChanges ();
        }

        public void UpdateLendBook(LendBook book)
        {
            _context.LendBookTable.Update(book);
        }

        public LendBook? GetLendBookById(int bookId)
        {
            return _context.LendBookTable.Find(bookId);
        }

        
    }
}
