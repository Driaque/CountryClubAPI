using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CountryClubMVC.Models;
using CountryClubMVC.ViewModels;

namespace CountryClubMVC.Controllers
{
    public class UsersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.family);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Login()
        {

            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
       
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userdetails = db.Users.Where(x => x.Username == user.Username).SingleOrDefault(i => i.Username == user.Username && i.Password == user.Password); ;
           
                if (userdetails != null)
                {
                    Session["User"] = userdetails;
                    Session["USERID"] = userdetails.User_ID;
                    Session["FAMID"] = userdetails.Family_ID;

                    return RedirectToAction("Index", "Home");

                }
                ModelState.AddModelError("", "Invalid Credentials");
        
            return View();
        }

        // GET: Users/Register
        public ActionResult Register()
       {
            ViewBag.Family_ID = new SelectList(db.Familys, "Family_ID", "FamilyName");
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "User_ID,Username,Password,Firstname,Lastname,Gender,Email,PhoneNumber,Town,Street,Country,PostalCode,Family_ID,Title,DateOfBirth,DisplayPicture,DateJoined,IsPasswordReset")] User user)
        {
            if (ModelState.IsValid)
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (user.Family_ID == null)
                {
                    Family family = new Family();
                    family.Family_ID = GenerateFamilyCode(user.Lastname);
                    family.FamilyName = user.Lastname;
                    family.MemberCount = family.MemberCount + 1;
                    db.Familys.Add(family);
                    user.Family_ID = family.Family_ID;
                }
                else
                {
                    var family = db.Familys.Where(x => x.Family_ID == user.Family_ID).SingleOrDefault();
                    family.MemberCount = family.MemberCount + 1;
                }


                user.DateJoined = DateTime.Now.ToShortDateString();

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Family_ID = new SelectList(db.Familys, "Family_ID", "FamilyName", user.Family_ID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Family_ID = new SelectList(db.Familys, "Family_ID", "FamilyName", user.Family_ID);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "User_ID,Username,Password,Firstname,Lastname,Gender,Email,PhoneNumber,Town,Street,Country,PostalCode,Family_ID,Title,DateOfBirth,DisplayPicture,DateJoined,IsPasswordReset")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Family_ID = new SelectList(db.Familys, "Family_ID", "FamilyName", user.Family_ID);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string GenerateFamilyCode(string lastname)
        {
            var guid = Guid.NewGuid();
            string finalCode = lastname.Substring(0, 3) + guid.ToString().Substring(0, 7);
            return finalCode;
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
