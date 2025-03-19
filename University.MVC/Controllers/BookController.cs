using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using University.BLL.Interfaces;
using University.DAL.Models;

namespace University.MVC.Controllers
{
    public class BookController : Controller
    {
        private readonly  IBookBLL _bookBLL;
        private readonly IUserBLL _userBLL;
        public BookController(IBookBLL bookBLL,
            IUserBLL userBLL)
        {
            _bookBLL = bookBLL;
            _userBLL = userBLL;
        }

        //[Authorize(Policy="Read_Book")]
        public IActionResult Index(string searchString)//here id is userId
        {
            var books = _bookBLL.GetAllBook();
            if(!searchString.IsNullOrEmpty())
            {
                books=books.Where(
                    b=>b.Name!.Contains(searchString)
                 || b.Writer!.Contains(searchString) 
                 || b.Publication!.Contains(searchString)
                 ).ToList();
            }
            return View(books);
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
            var available = _bookBLL.CheckStock(target.Name,target.Writer);
            if (available.Count() is 0)
            {
                ViewBag.quentity = 0;
            }
            else
            {
                ViewBag.quentity = available.Count();
                ViewBag.id=available.First().Id;
            }
            //ViewBag.Available=available
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

        public IActionResult StudentDetails(int studentId)
        {
            if (studentId <= 0)
                return NotFound();
            var student=_bookBLL.GetStudentById(studentId);
            return View(student);
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
