using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CountryClubMVC.Models;

namespace CountryClubMVC.Controllers
{
    public class FollowershipsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Followerships
        public ActionResult Index()
        {
            return View(db.Followerships.ToList());
        }

        // GET: Followerships/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Followership followership = db.Followerships.Find(id);
            if (followership == null)
            {
                return HttpNotFound();
            }
            return View(followership);
        }

        // GET: Followerships/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Followerships/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Follow(int id)
        {
            Followership followership = new Followership();
            //Get the current user id from session and make it follower ID\
            followership.FollowerID = Convert.ToInt32(Session["USERID"]);
            followership.isFollowing = true;
            followership.UserID = id;
            if (ModelState.IsValid)
            {
                //add record of following
                db.Followerships.Add(followership);

                //Nofity Followed user
                var notification = new Notification();
                notification.User_ID = followership.UserID;
                var followingUser = db.Users.Find(followership.FollowerID);

                notification.Message = $"{followingUser.Username} has started following you!";
                notification.Time = DateTime.Now.ToString();
                db.Notifications.Add(notification);


                //save changes to db
                db.SaveChanges();
                return RedirectToAction("People", "Users");
            }

            return View(followership);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnFollow(int id)
        {
            var followerID = Convert.ToInt32(Session["USERID"]);
            var userID = id;
            //Get the current user id from session and make it follower ID\
            //Followership followership = db.Followerships.Find(id);
            Followership followership = db.Followerships.Where(x =>x.UserID == userID && x.FollowerID == followerID).FirstOrDefault();


            if (ModelState.IsValid)
            {
                //delete record of following
                db.Followerships.Remove(followership);
                //save changes to db
                db.SaveChanges();
                return RedirectToAction("People", "Users");
            }

            return View(followership);
        }

        public ActionResult FollowUser(int? id)
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

            return View();
        }

        // GET: Followerships/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Followership followership = db.Followerships.Find(id);
            if (followership == null)
            {
                return HttpNotFound();
            }
            return View(followership);
        }

        // POST: Followerships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FollowershipID,UserID,FollowerID,Status")] Followership followership)
        {
            if (ModelState.IsValid)
            {
                db.Entry(followership).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(followership);
        }

        // GET: Followerships/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Followership followership = db.Followerships.Find(id);
            if (followership == null)
            {
                return HttpNotFound();
            }
            return View(followership);
        }

        // POST: Followerships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Followership followership = db.Followerships.Find(id);
            db.Followerships.Remove(followership);
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
