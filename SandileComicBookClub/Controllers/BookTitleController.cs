using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SandileComicBookClub.Overrides;
using SandileComicBookClub.Models;


namespace SandileComicBookClub.Controllers
{
    [AuthorizeAdmin(Roles = "Admin")]
    public class BookTitleController : Controller
    {
        private ComicBookClubDB db = new ComicBookClubDB();

        // GET: BookTitle
        public ActionResult Index()
        {
            var bookTitles = db.BookTitles.Include (b => b.Category);
            return View(bookTitles.ToList());
        }

        // GET: BookTitle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTitle bookTitle = db.BookTitles.Find(id);
            if (bookTitle == null)
            {
                return HttpNotFound();
            }
            return View(bookTitle);
        }

        // GET: BookTitle/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: BookTitle/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TitleID,Title,Series,Author,Date Published,Books Available,CategoryID,GameBoxArtURL")] BookTitle bookTitle)
        {
            if (ModelState.IsValid)
            {
                if (bookTitle.Books_Available == 1)
                {
                    ModelState.AddModelError("", "At least borrow one comic book.");
                    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", bookTitle.CategoryID);
                    return View(bookTitle);
                }
                db.BookTitles.Add(bookTitle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", bookTitle.CategoryID);
            return View(bookTitle);
        }

        // GET: BookTitle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTitle bookTitle = db.BookTitles.Find(id);
            if (bookTitle == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", bookTitle.CategoryID);
            return View(bookTitle);
        }

        // POST: BookTitle/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TitleID,Title,Series,Author,Date Published,Books Available,CategoryID,GameBoxArtURL")] BookTitle bookTitle)
        {
            if (ModelState.IsValid)
            {
                if (bookTitle.Books_Available == 1)
                {
                    ModelState.AddModelError("", "At least borrow one comic book");
                    ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", bookTitle.CategoryID);
                    return View(bookTitle);
                }
                db.Entry(bookTitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", bookTitle.CategoryID);
            return View(bookTitle);
        }

        // GET: BookTitle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookTitle bookTitle = db.BookTitles.Find(id);
            if (bookTitle == null)
            {
                return HttpNotFound();
            }
            return View(bookTitle);
        }

        // POST: BookTitle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookTitle bookTitle = db.BookTitles.Find(id);
            db.BookTitles.Remove(bookTitle);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
