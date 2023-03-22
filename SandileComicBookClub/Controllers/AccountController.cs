using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Principal;
using SandileComicBookClub.Overrides;
using SandileComicBookClub.Models;

namespace SandileComicBookClub.Controllers
{
    public class AccountController : Controller
    {
        private ComicBookClubDB db = new ComicBookClubDB();

        [AllowAnonymous]
        //GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        // POST: Account/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Username,Password")] Member member)
        {
            member.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Form["Password"], "SHA1");
            if (member.Password != "" && member.Username != "")
            {
                if (member.Username.ToLowerInvariant() == User.Identity.Name.ToLowerInvariant())
                {
                    ModelState.AddModelError("", "You are already logged in.");
                    return View("Login");
                }

                Member userMatch = (from s in db.Members
                                    where s.Password == member.Password && s.Username == member.Username
                                    select s).FirstOrDefault();

                if (userMatch != null)
                {
                   
                    bool persist = false;
                    if (Request.Form["RememberMe"] == "on")
                        persist = true;

                    FormsAuthentication.SetAuthCookie(member.Username, persist);
                    if (!Roles.IsUserInRole(member.Username, "Admin") && userMatch.Administrator)
                        Roles.AddUserToRole(member.Username, "Admin");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    var list = from s in db.Members
                               select s;

                    if (list.Count() == 0)
                    {
                        ModelState.AddModelError("", "The database is empty. Please create an account.");
                        return View("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect username/password combination.");
                        return View("Login");
                    }

                }
            }
            else
            {
                if (member.Password == "")
                    ModelState.AddModelError("Password", "Please enter your Password.");
                else
                    ModelState.AddModelError("Username", "Please enter your Username.");
                return View("Login");
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AuthorizeAdmin(Roles = "Admin")]
       
        // GET: Account
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        [Authorize]
        // GET: Account/Details/5
        public ActionResult Details(int? id)
        {
            Member member;
            if (id == null)
            {
                member = getUserRecord();
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
                member = db.Members.Find(id);

            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        [AllowAnonymous]
        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "Username,Password,Name,Surname,Email,CellPhone")] Member member)
        {
            if (ModelState.IsValid)
            {
                Member unique = (from s in db.Members
                                 where s.Username == member.Username
                                 select s).FirstOrDefault();
                if (unique == null)
                {
                    if (member.Password != Request.Form["Confirm-Password"])
                    {
                        ModelState.AddModelError("", "Passwords do not match");
                        ViewBag.confirmPW = Request.Form["Confirm-Password"];
                        return View(member);
                    }
                    member.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(member.Password, "SHA1");

                    //Seed DB with first Administrator
                    if (member.Username.ToLowerInvariant() == "admin") member.Administrator = true;

                    db.Members.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.confirmPW = Request.Form["Confirm-Password"];
                    ModelState.AddModelError("Username", "Username is taken");
                    return View(member);
                }
            }
            ViewBag.confirmPW = Request.Form["Confirm-Password"];
            return View(member);
        }

        [Authorize]
        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            //Determine if it's a Member or Admin Operation
            Member member = getUserRecord();
            if (id != member.MemberID && !Roles.IsUserInRole(member.Username, "Admin"))
            {
                return View("Unauthorized");
            }

            if (id != null)
            {
                member = db.Members.Find(id);
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (member == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Admin"))
                return View("Edit_admin", member);
            else
                return View(member);
        }

        // GET: Account/Edit_admin
        public ActionResult Edit_admin(int? id)
        {
            return RedirectToAction("Edit", id);
        }


        [Authorize]
        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MemberID,Username,Password,Name,Surname,Email,CellPhone")] Member changed)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form["PWchange"] == "1" && !(Request.Form["CurrentPassword"] == "" && Request.Form["NewPassword"] == "" && Request.Form["ConfirmPassword"] == ""))
                {
                    string oldHashed = FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Form["CurrentPassword"], "SHA1");
                    if (oldHashed != changed.Password)
                    {
                        ModelState.AddModelError("", "Current Password was incorrect.");
                        ModelState.AddModelError("", "Please try again.");
                        return View(changed);
                    }
                    string newPW = Request.Form["NewPassword"];
                    string confirmNewPW = Request.Form["ConfirmPassword"];
                    if (newPW.Length == 0)
                    {
                        ModelState.AddModelError("", "The New Password field is required.");
                        ModelState.AddModelError("", "Please try again.");
                        return View(changed);
                    }
                    if (!newPW.Equals(confirmNewPW))
                    {
                        ModelState.AddModelError("", "New and Confirm New passwords did not match.");
                        ModelState.AddModelError("", "Please try again.");
                        return View(changed);
                    }

                    changed.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPW, "SHA1");


                }
                db.Entry(changed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Account", new { id = changed.MemberID });
            }
            return View(changed);
        }

        [AuthorizeAdmin(Roles = "Admin")]
        // POST: Account/Edit_admin/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_admin([Bind(Include = "MemberID,Username,Password,Name,Surname,Email,CellPhone,Administrator,Banned")] Member changed)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form["PWchange"] == "1" && !(Request.Form["NewPassword"] == "" && Request.Form["ConfirmPassword"] == ""))
                {
                    string newPW = Request.Form["NewPassword"];
                    string confirmNewPW = Request.Form["ConfirmPassword"];
                    if (newPW.Length == 0)
                    {
                        ModelState.AddModelError("", "The New Password field is required.");
                        ModelState.AddModelError("", "Please try again.");
                        return View(changed);
                    }
                    if (!newPW.Equals(confirmNewPW))
                    {
                        ModelState.AddModelError("", "The New and Confirm New passwords did not match.");
                        ModelState.AddModelError("", "Please try again.");
                        return View(changed);
                    }

                    changed.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPW, "SHA1");


                }
                db.Entry(changed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(changed);
        }

        [AuthorizeAdmin(Roles = "Admin")]
        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Member member = db.Members.Find(id);

            if (member == null)
            {
                return HttpNotFound();
            }

            return View(member);
        }

        [AuthorizeAdmin(Roles = "Admin")]
        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member current = getUserRecord();
            if (id == current.MemberID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Member getUserRecord()
        {
            if (!Request.IsAuthenticated)
            {
                return null;
            }

            Member user = (from s in db.Members
                           where s.Username == User.Identity.Name
                           select s).First();

            return user;
        }
    }
}
