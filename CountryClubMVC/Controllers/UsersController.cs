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


        // GET: People
        public ActionResult People(string searchString)
        {
            var user = from xm in db.Users
                         select xm;

            if (!string.IsNullOrEmpty(searchString))
            {
                user = user.Where(o => o.Firstname.Contains(searchString));

            }


           
            return View(user.ToList());
        }

        public  ActionResult FollowUser(int? id)
        {
            //Get the current user id from session and make it follower ID\
            var followerID = Session["USERID"];
            // For following Query follower table to get every follower id that contiains (user_id)


            var userprofile = db.Users.Find(id);
            var follower = db.Users.Find(followerID);
            //add record of following
            userprofile.Followers.Add(follower);
            //save changes to db
            db.SaveChanges();
           
            
            // return RedirectToAction("Index", "Posts");
                       
            return View();
        }


        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.family);
            return View(users.ToList());
        }

        // GET: GetFamilyMemebers
        public ActionResult GetFamilyMembers()
        {
            var FamilyID = Session["FAMID"].ToString();
            var users = db.Users.Where(x => x.Family_ID == FamilyID);
            return View(users.ToList());
        }


        // GET: Users/Profile/5
        public new ActionResult Profile(int? id)
        {
            Session["ProfileID"] = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
           
            if (user == null)
            {
                return HttpNotFound();
            }
            var userimage = db.Users.Where(x => x.User_ID == user.User_ID).SingleOrDefault();
            return View(user);
        }

        //GET: AddFamily
        public ActionResult AddFamily()
        {

            return View();
        }
        // POST: AddFamily
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFamily(User user)
        {
            if (ModelState.IsValid)
            {
                if (!ModelState.IsValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (user.Family_ID == null)
                {
                    user.Family_ID = Session["FAMID"].ToString();
                    user.Lastname = Session["FAMNAME"].ToString();

                    //
                    var family = db.Familys.Where(x => x.Family_ID == user.Family_ID).SingleOrDefault();

                    family.MemberCount = family.MemberCount + 1;

                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");

                }

                user.DateJoined = DateTime.Now.ToShortDateString();

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("GetFamilyMembers", "Users");
            }

            ViewBag.Family_ID = new SelectList(db.Familys, "Family_ID", "FamilyName", user.Family_ID);
            return View(user);
        }

        //GET: Login
        public ActionResult Login()
        {
            AppDbContext c = new AppDbContext();
            c.Database.CreateIfNotExists();

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
                Session["FAMTIT"] = userdetails.Title;
                Session["FAMNAME"] = userdetails.Lastname;

                return RedirectToAction("Index", "Posts");

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
            try
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

                        Session["RegistrationResponse"] = "Success"; //To be passed to a modal


                    }
                    else
                    {
                        var family = db.Familys.Where(x => x.Family_ID == user.Family_ID).SingleOrDefault();
                        family.MemberCount = family.MemberCount + 1;
                    }

                    user.DateJoined = DateTime.Now.ToShortDateString();

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Users");
                }
            }
            catch (Exception)
            {
                Session["RegistrationResponse"] = "Please Complete Form !";
                return View(new RegisterViewModel { });
                //throw;
            }

            ViewBag.Family_ID = new SelectList(db.Familys, "Family_ID", "FamilyName", user.Family_ID);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult EditProfile(int? id)
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
        public ActionResult EditProfile([Bind(Include = "User_ID,Username,Password,Firstname,Lastname,Gender,Email,PhoneNumber,Town,Street,Country,PostalCode,Family_ID,Bio,Title,DateOfBirth,DisplayPicture,CoverPicture,DateJoined,IsPasswordReset")] User user)
        {
              if (ModelState.IsValid)
                {
                    

                    db.Entry(user).State = EntityState.Modified;
                    var entry = db.Entry(user);
                    entry.Property(e => e.DateJoined).IsModified = false;
                    entry.Property(e => e.Family_ID).IsModified = false;
                    entry.Property(e => e.Gender).IsModified = false;
                    entry.Property(e => e.IsPasswordReset).IsModified = false;
                    entry.Property(e => e.Lastname).IsModified = false;
                    entry.Property(e => e.Password).IsModified = false;
                    entry.Property(e => e.Title).IsModified = false;
                    entry.Property(e => e.DateOfBirth).IsModified = false;
                // db.Users.Add(user);
                    db.SaveChanges();
                    var UserID = Session["USERID"].ToString();
                    var profileLink = "Profile/" + UserID;
                    return RedirectToAction(profileLink);
                }
            
           
            return View();

         

        }

        //GET: Login
        public ActionResult Settings(int? id)
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

        // POST: Login
        [HttpPost]
        public ActionResult Settings(SettingsViewModel user)
        {

            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userdetails = db.Users.Where(x => x.Password == user.Password).SingleOrDefault(i =>  i.Password == user.Password); ;

            if (userdetails != null)
            {
                Session["User"] = userdetails;
                Session["USERID"] = userdetails.User_ID;
                Session["FAMID"] = userdetails.Family_ID;
                Session["FAMTIT"] = userdetails.Title;
                Session["FAMNAME"] = userdetails.Lastname;

                return RedirectToAction("Index", "Posts");

            }
            ModelState.AddModelError("", "Invalid Credentials");

            return View();
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

        public string FollowCount()
        {

            var followCount = db.Followerships.Count();
           
            return followCount.ToString();
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
