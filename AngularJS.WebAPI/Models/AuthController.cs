using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;

namespace AngularJS.WebAPI.Models
{
    public class AuthController : ApiController
    {
        private AngularJSWebAPIContext db = new AngularJSWebAPIContext();


        [Route("Auth/Login")]
        public string Auth(Credentials cred)
        {

            var r= db.Credentials.Select(x => x.UserName.Equals(cred.UserName) && x.Password.Equals(cred.Password)).Count() == 1;
            var result = new { Result = r };
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
        [Route("Auth/LogOut")]
        [HttpGet]
        public string LogOut()
        {
            return string.Empty;
        }
        // GET api/Auth
        public IQueryable<Credentials> GetCredentials()
        {
            return db.Credentials;
        }

        // GET api/Auth/5
        [ResponseType(typeof(Credentials))]
        public IHttpActionResult GetCredentials(int id)
        {
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return NotFound();
            }

            return Ok(credentials);
        }

        // PUT api/Auth/5
        public IHttpActionResult PutCredentials(int id, Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != credentials.Id)
            {
                return BadRequest();
            }

            db.Entry(credentials).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CredentialsExists(id))
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

        // POST api/Auth
        [ResponseType(typeof(Credentials))]
        public IHttpActionResult PostCredentials(Credentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Credentials.Add(credentials);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = credentials.Id }, credentials);
        }

        // DELETE api/Auth/5
        [ResponseType(typeof(Credentials))]
        public IHttpActionResult DeleteCredentials(int id)
        {
            Credentials credentials = db.Credentials.Find(id);
            if (credentials == null)
            {
                return NotFound();
            }

            db.Credentials.Remove(credentials);
            db.SaveChanges();

            return Ok(credentials);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CredentialsExists(int id)
        {
            return db.Credentials.Count(e => e.Id == id) > 0;
        }
    }
}