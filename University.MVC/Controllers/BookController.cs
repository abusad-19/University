using Microsoft.AspNetCore.Mvc;
using University.BLL.Services;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class BookController : Controller
    {
        private readonly  BookBLL _bookBLL;
        public BookController(BookBLL bookBLL)
        {
            _bookBLL = bookBLL;
        }
        public IActionResult Index()
        {
            return View(_bookBLL.GetAllBook());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Post(Book book)
        {
            if(book is null)
                return NotFound();
            await _bookBLL.AddBookAsync(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id <=0)
                return NotFound();

            return View(_bookBLL.GetBookById(id));
        }

        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> Update(Book book)
        {
            if(book is null)
                return NotFound();

            await _bookBLL.UpdateAsync(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if(id<=0)
                return NotFound();

            var target=_bookBLL.GetBookById(id);
            return View(target);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            if(id<=0)
                return NotFound();

            await _bookBLL.DeleteAsync(id);
            return RedirectToAction(nameof(Index)); 
        }

        public IActionResult Details(int id)
        {
            if(id<=0)
                return NotFound();

            var target=_bookBLL.GetBookById(id);
            return View(target);
        }

        public IActionResult AddToCart(int id)
        {
            if(id<=0)
                return NotFound();
            
            _bookBLL.AddCart(id);
            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Cart()
        {
            var addedToCart= _bookBLL.GetCart();
            return View(addedToCart);
        }

        public IActionResult RemoveFromCart(int id)//here id is BookId, not Cart Id
        {
            if(id<=0)
                return NotFound();
            bool isPossible=_bookBLL.RemoveCart(id);
            if(isPossible is false)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Cart));
        }

        public IActionResult LendBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LendBook(int studentId)//here id is studentId
        {
            if(studentId<=0)
                return NotFound();
            var cart = _bookBLL.GetCart();
            var student = _bookBLL.GetStudentById(studentId);
            if (cart is null || student is null)
                return NotFound();

            _bookBLL.LendBook(student, cart);
            
            return RedirectToAction(nameof(ReturnBook), new {studentId=studentId});
        }

        public IActionResult GotoReturnPage()
        {
            return View();
        }

        public IActionResult ReturnBook(int studentId)
        {
            if(studentId<=0)
                return NotFound();
            
            var books=_bookBLL.GetLendBooks(studentId);
            return View(books);
        }

        public IActionResult ConfirmReturnBook(int id)// here id is LendBookId
        {
            if(id<=0)
                return NotFound();
           
            var studentId = _bookBLL.ReturnLendBook(id);
            if(studentId is null)
                return NotFound();

            return RedirectToAction(nameof(ReturnBook), new { studentId =studentId});
        }

        
    }
}
