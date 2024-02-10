using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ridesnShare.Models;

namespace ridesnShare.Controllers
{
    public class DriverDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves a list of drivers from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of DriverDTO objects representing the list of drivers.
        /// </returns>
        /// <example>
        /// GET: api/DriverData/ListDrivers
        /// </example>
        [HttpGet]
        [Route("api/DriverData/ListDrivers")]
        public IEnumerable<DriverDTO> Drivers()
        {
            List<Driver> Drivers = db.Drivers.ToList();
            List<DriverDTO> DriverDTOs = new List<DriverDTO>();

            Drivers.ForEach(d => DriverDTOs.Add(new DriverDTO()
            {
                DriverId = d.DriverId,
                firstName = d.firstName,
                lastName = d.lastName,
                email = d.email
            }));

            return DriverDTOs;

        }

        // GET: api/DriverData/5
        [ResponseType(typeof(Driver))]
        public IHttpActionResult FindDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }

        // PUT: api/DriverData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDriver(int id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driver.DriverId)
            {
                return BadRequest();
            }

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        /// <summary>
        /// Adds a new driver to the database.
        /// </summary>
        /// <param name="driver">The driver object containing information about the new driver.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the addition operation.
        /// </returns>
        /// <example>
        /// POST: api/DriverData/AddDriver/5
        /// </example>
        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult AddDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = driver.DriverId }, driver);
        }

        // DELETE: api/DriverData/5
        [ResponseType(typeof(Driver))]
        public IHttpActionResult DeleteDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            db.SaveChanges();

            return Ok(driver);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.DriverId == id) > 0;
        }
    }
}