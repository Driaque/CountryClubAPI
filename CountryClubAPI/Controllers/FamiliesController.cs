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
    builder.EntitySet<Family>("Families");
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FamiliesController : ODataController
    {
        private CountryClubEntities db = new CountryClubEntities();

        // GET: odata/Families
        [EnableQuery]
        public IQueryable<Family> GetFamilies()
        {
            return db.Families;
        }

        // GET: odata/Families(5)
        [EnableQuery]
        public SingleResult<Family> GetFamily([FromODataUri] string key)
        {
            return SingleResult.Create(db.Families.Where(family => family.Family_ID == key));
        }

        // PUT: odata/Families(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Family> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Family family = db.Families.Find(key);
            if (family == null)
            {
                return NotFound();
            }

            patch.Put(family);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(family);
        }

        // POST: odata/Families
        public IHttpActionResult Post(Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Families.Add(family);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (FamilyExists(family.Family_ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(family);
        }

        // PATCH: odata/Families(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Family> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Family family = db.Families.Find(key);
            if (family == null)
            {
                return NotFound();
            }

            patch.Patch(family);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(family);
        }

        // DELETE: odata/Families(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            Family family = db.Families.Find(key);
            if (family == null)
            {
                return NotFound();
            }

            db.Families.Remove(family);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/Families(5)/Users
        [EnableQuery]
        public IQueryable<User> GetUsers([FromODataUri] string key)
        {
            return db.Families.Where(m => m.Family_ID == key).SelectMany(m => m.Users);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FamilyExists(string key)
        {
            return db.Families.Count(e => e.Family_ID == key) > 0;
        }
    }
}
