using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SandileComicBookClub.Models;
using System.Globalization;

namespace SandileComicBookClub.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private ComicBookClubDB db = new ComicBookClubDB();

        public ActionResult Index()
        {
            List<string> query = (from s in db.Categories
                                  orderby s.Name
                                  select s.Name).ToList();
            query.Insert(0, "All");
            ViewBag.categories = new SelectList(query, "All");

            var selection = from s in db.BookTitles
                            orderby s.Title
                            select s;

            return View(selection);
        }

        // POST: Home/Explore
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Explore(object categories)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form["Category"] == "All")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    string category = Request.Form["Category"];

                    List<string> query = (from s in db.Categories
                                          orderby s.Name
                                          select s.Name).ToList();
                    query.Insert(0, "All");
                    ViewBag.categories = new SelectList(query, category);

                    var selection = from s in db.BookTitles
                                    where s.Category.Name == category
                                    orderby s.Title
                                    select s;

                    return View("Index", selection);
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult About()
        {
            return View();
        }


        [Authorize]
        public ActionResult Returning()
        {
            string date = DateTime.Today.ToShortDateString();
            var books = from s in db.Customers
                        where s.MemberInfo.Username == User.Identity.Name && s.BookBorrowing.BorrowDate.CompareTo(date) >= 0
                        orderby s.BookBorrowing.BorrowDate descending
                        select s;
            var List = new List<BorrowedBook>();
            foreach (Customer p in books)
            {
                List.Add(p.BookBorrowing);
            }
            return PartialView(List);
        }

        [Authorize]
        public ActionResult BorrowedBooks()
        {
            string date = DateTime.Today.ToShortDateString();
            var books = from s in db.BorrowedBooks
                        where s.BorrowDate.CompareTo(date) >= 0
                        orderby s.BorrowDate descending
                        select s;
            return PartialView(books);
        }

    }
}