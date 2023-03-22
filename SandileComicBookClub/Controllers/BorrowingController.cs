using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SandileComicBookClub.Models;

namespace SandileComicBookClub.Controllers
{
    [Authorize]
    public class BorrowingController : Controller
    {

        private ComicBookClubDB db = new ComicBookClubDB();

        // GET: Borrowing
        public ActionResult Index()
        {
            return View();
        }

        // GET: Borrowing/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BorrowedBook borrowedBook = db.BorrowedBooks.Find(id);
            if (borrowedBook == null)
            {
                return HttpNotFound();
            }
            return View(borrowedBook);
        }


        // GET: Category/Create
        [Authorize]
        public ActionResult Borrow (int id)
        {
            var book = db.BookTitles.FirstOrDefault(x => x.TitleID==id);
            return View(book);
        }

        // GET: Category/ Checking out when borrowing a book
        public ActionResult Checkout (int id)
        {
            var user = User.Identity.Name;
            var book = new BorrowedBook { 
            TitleID = id,
            BorrowDate = DateTime.Now.ToString(),
            UserName = user,
            Status = "Borrowed", 
            ReturnDate = "", 
            Time = DateTime.Now.ToShortTimeString(), 

            };
            db.BorrowedBooks.Add(book);
            db.SaveChanges();
            return RedirectToAction("MyBooks"); 
        }

        // Borrowing a book
        [Authorize]
        public ActionResult MyBooks ()
        {
            var user = User.Identity.Name;
           
            var books = db.BorrowedBooks.Where(x=>x.UserName == user && x.Status=="Borrowed").ToList();
            var booksVM = new List<BorrowedBookVM>();

            if (books.Any())
            {
                books.ForEach(book =>
                {
                    var bookVM = new BorrowedBookVM
                    {
                        TitleID = book.TitleID,
                        BorrowDate = book.BorrowDate,
                        UserName = book.UserName,
                        Status = book.Status,
                        ReturnDate = book.ReturnDate,
                        Time = book.Time,
                        BookTitle = db.BookTitles.FirstOrDefault(x=>x.TitleID == book.TitleID)

                    };
                    booksVM.Add(bookVM);
                });
            }
            return View (booksVM);
        }

        //Returning a borrowed Book
        [Authorize]
        public ActionResult MyReturnedBooks()
        {
            var user = User.Identity.Name;

            var books = db.BorrowedBooks.Where(x => x.UserName == user && x.Status == "Returned").ToList();
            var booksVM = new List<BorrowedBookVM>();

            if (books.Any())
            {
                books.ForEach(book =>
                {
                    var bookVM = new BorrowedBookVM
                    {
                        TitleID = book.TitleID,
                        BorrowDate = book.BorrowDate,
                        UserName = book.UserName,
                        Status = book.Status,
                        ReturnDate = book.ReturnDate,
                        Time = book.Time,
                        BookTitle = db.BookTitles.FirstOrDefault(x => x.TitleID == book.TitleID)

                    };
                    booksVM.Add(bookVM);
                });
            }
            return View(booksVM);
        }


        //Listing of all the books that has been borrow on the manage admin page
        [Authorize]
        public ActionResult AllBorrowedBooks()
        {
            
            var books = db.BorrowedBooks.Where(x => x.Status == "Borrowed").ToList();
            var booksVM = new List<BorrowedBookVM>();

            if (books.Any())
            {
                books.ForEach(book =>
                {
                    var bookVM = new BorrowedBookVM
                    {
                        TitleID = book.TitleID,
                        BorrowDate = book.BorrowDate,
                        UserName = book.UserName,
                        Status = book.Status,
                        ReturnDate = book.ReturnDate,
                        Time = book.Time,
                        BookTitle = db.BookTitles.FirstOrDefault(x => x.TitleID == book.TitleID)

                    };
                    booksVM.Add(bookVM);
                });
            }
            return View(booksVM);
        }
    }
}
