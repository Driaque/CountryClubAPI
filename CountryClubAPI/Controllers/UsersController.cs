using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Description;
using CountryClubAPI.Models;

namespace CountryClubAPI.Controllers
{
    public class UsersController : ApiController
    {
        private CountryClubEntities db = new CountryClubEntities();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.User_ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        [HttpPost]
        [ResponseType(typeof(AuthHelper))]
        [Route("api/Users/Login")]
        public string Login(User user)
        {
            AuthHelper auth = new AuthHelper();
            //Get User
            var credentials = user.Username;//parameters["Credentialss"] as ICollection<string>;
            var username = user.Username; //parameters["Username"] as string;
            var password = user.Password; //parameters["Password"] as string;

            if (!ModelState.IsValid)
            {
                // BadRequest(ModelState);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var userdetails = db.Users.Where(x => x.Username == username).SingleOrDefault(i => i.Username == username && i.Password == password); ;
            if (userdetails != null)
            {
                auth.IsAuthenticated = true;
                auth.UserID = userdetails.User_ID;
                auth.FamilyID = userdetails.Family_ID;
                //auth.User = userdetails;
            }
            else
            {
                auth.IsAuthenticated = false;
            }

            string jsonData =Newtonsoft.Json.JsonConvert.SerializeObject(auth);
            string escapedJsonData = Regex.Escape(jsonData);

            return escapedJsonData;
        }



        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user.Family_ID == null)
            {
                Family family = new Family();
                family.Family_ID = GenerateFamilyCode(user.Lastname);
                family.FamilyName = user.Lastname;
                family.MemberCount = family.MemberCount + 1;
                db.Families.Add(family);
                user.Family_ID = family.Family_ID;
            }
            else
            {
                var family = db.Families.Where(x => x.Family_ID == user.Family_ID).SingleOrDefault();
                family.MemberCount = family.MemberCount + 1;
            }


            user.DateJoined = DateTime.Now.ToShortDateString();

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.User_ID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.User_ID == id) > 0;
        }

        public string GenerateFamilyCode(string lastname)
        {
            var guid = Guid.NewGuid();
            string finalCode = lastname.Substring(0, 3) + guid.ToString().Substring(0, 7);
            return finalCode;
        }

    }
}