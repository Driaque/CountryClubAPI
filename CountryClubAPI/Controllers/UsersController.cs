using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using CountryClubAPI.Models;

namespace CountryClubAPI.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using CountryClubAPI.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<User>("Users");
    builder.EntitySet<Comment>("Comments"); 
    builder.EntitySet<Family>("Families"); 
    builder.EntitySet<Friend>("Friends"); 
    builder.EntitySet<Message>("Messages"); 
    builder.EntitySet<Post>("Posts"); 
    builder.EntitySet<PostLikedbyUser>("PostLikedbyUsers"); 
    builder.EntitySet<User_Has_Interest>("User_Has_Interest"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UsersController : ODataController
    {
        private CountryClubEntities db = new CountryClubEntities();

        // GET: odata/Users
        [EnableQuery]
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: odata/Users(5)
        [EnableQuery]
        public SingleResult<User> GetUser([FromODataUri] int key)
        {
            return SingleResult.Create(db.Users.Where(user => user.User_ID == key));
        }

        // PUT: odata/Users(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<User> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = db.Users.Find(key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Put(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(user);
        }

        // POST: odata/Users
        //[RoutePrefix("api/User")]
        public IHttpActionResult Post(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(user.Family_ID == null)
            {
                Family family = new Family();
                family.Family_ID = GenerateFamilyCode(user.Lastname);
                family.FamilyName = user.Lastname;
                family.MemberCount = family.MemberCount + 1;
                db.Families.Add(family);
                user.Family_ID = family.Family_ID;
            }
            else {
                var family = db.Families.Where(x => x.Family_ID == user.Family_ID).SingleOrDefault();
                family.MemberCount = family.MemberCount + 1;
            }
            

            user.DateJoined = DateTime.Now.ToShortDateString();

            
            db.Users.Add(user);
            db.SaveChanges();

            return Created(user);
        }

        // PATCH: odata/Users(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<User> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = db.Users.Find(key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Patch(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(user);
        }

        // DELETE: odata/Users(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            User user = db.Users.Find(key);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Users(5)/Comments
        [EnableQuery]
        public IQueryable<Comment> GetComments([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.Comments);
        }

        // GET: odata/Users(5)/Family
        [EnableQuery]
        public SingleResult<Family> GetFamily([FromODataUri] int key)
        {
            return SingleResult.Create(db.Users.Where(m => m.User_ID == key).Select(m => m.Family));
        }

        // GET: odata/Users(5)/Friends
        [EnableQuery]
        public IQueryable<Friend> GetFriends([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.Friends);
        }

        // GET: odata/Users(5)/Friends1
        [EnableQuery]
        public IQueryable<Friend> GetFriends1([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.Friends1);
        }

        // GET: odata/Users(5)/Messages
        [EnableQuery]
        public IQueryable<Message> GetMessages([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.Messages);
        }

        // GET: odata/Users(5)/Messages1
        [EnableQuery]
        public IQueryable<Message> GetMessages1([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.Messages1);
        }

        // GET: odata/Users(5)/Posts
        [EnableQuery]
        public IQueryable<Post> GetPosts([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.Posts);
        }

        // GET: odata/Users(5)/PostLikedbyUsers
        [EnableQuery]
        public IQueryable<PostLikedbyUser> GetPostLikedbyUsers([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.PostLikedbyUsers);
        }

        // GET: odata/Users(5)/User_Has_Interest
        [EnableQuery]
        public IQueryable<User_Has_Interest> GetUser_Has_Interest([FromODataUri] int key)
        {
            return db.Users.Where(m => m.User_ID == key).SelectMany(m => m.User_Has_Interest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int key)
        {
            return db.Users.Count(e => e.User_ID == key) > 0;
        }
        public string GenerateFamilyCode(string lastname)
        {
            string finalCode = string.Empty;
            Guid guid = new Guid();
           
            finalCode = lastname.Substring(0, 3) + guid.ToString().Substring(0, 7);
            return finalCode;
        }

    }
}
