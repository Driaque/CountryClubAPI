using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
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
    builder.EntitySet<Interest>("Interests");
    builder.EntitySet<User_Has_Interest>("User_Has_Interest"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    [EnableCors(origins: "http://localhost:46204", headers: "*", methods: "*")]
    public class InterestsController : ODataController
    {
        private CountryClubEntities db = new CountryClubEntities();

        // GET: odata/Interests
        [EnableQuery]
        public IQueryable<Interest> GetInterests()
        {
            return db.Interests;
        }

        // GET: odata/Interests(5)
        [EnableQuery]
        public SingleResult<Interest> GetInterest([FromODataUri] int key)
        {
            return SingleResult.Create(db.Interests.Where(interest => interest.Interest_ID == key));
        }

        // PUT: odata/Interests(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Interest> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Interest interest = db.Interests.Find(key);
            if (interest == null)
            {
                return NotFound();
            }

            patch.Put(interest);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterestExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(interest);
        }
       
        // POST: odata/Interests
        public IHttpActionResult Post(Interest interest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Interests.Add(interest);
            db.SaveChanges();

            return Created(interest);
        }

        // PATCH: odata/Interests(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Interest> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Interest interest = db.Interests.Find(key);
            if (interest == null)
            {
                return NotFound();
            }

            patch.Patch(interest);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterestExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(interest);
        }

        // DELETE: odata/Interests(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Interest interest = db.Interests.Find(key);
            if (interest == null)
            {
                return NotFound();
            }

            db.Interests.Remove(interest);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Interests(5)/User_Has_Interest
        [EnableQuery]
        public IQueryable<User_Has_Interest> GetUser_Has_Interest([FromODataUri] int key)
        {
            return db.Interests.Where(m => m.Interest_ID == key).SelectMany(m => m.User_Has_Interest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InterestExists(int key)
        {
            return db.Interests.Count(e => e.Interest_ID == key) > 0;
        }
    }
}
