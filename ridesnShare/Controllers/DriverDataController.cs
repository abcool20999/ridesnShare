using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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
        /// <summary>
        /// Retrieves information about a specific driver from the database.
        /// </summary>
        /// <param name="id">The ID of the driver to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing information about the driver.
        /// </returns>
        /// <example>
        /// GET: api/DriverData/FindDriver/{id}
        /// </example>

        [ResponseType(typeof(Driver))]
        [HttpGet]
        [Route("api/DriverData/FindDriver/{id}")]
        public IHttpActionResult FindDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            DriverDTO driverDTO = new DriverDTO()
            {
                DriverId = driver.DriverId,
                firstName = driver.firstName,
                lastName = driver.lastName,
                email = driver.email
            };

            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driverDTO);
        }

        /// <summary>
        /// Updates information about a specific driver in the database.
        /// </summary>
        /// <param name="id">The ID of the driver to update.</param>
        /// <param name="driver">The updated information of the driver.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the update operation.
        /// </returns>
        /// <example>
        /// POST: api/DriverData/UpdateDriver/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/DriverData/UpdateDriver/{id}")]
        public IHttpActionResult UpdateDriver(int id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Model State is invalid");
                return BadRequest(ModelState);
            }

            if (id == default)
            {
                Debug.WriteLine("ID mismatch");
                Debug.WriteLine("GET parameter" + id);
                Debug.WriteLine("POST parameter" + driver.DriverId);
                Debug.WriteLine("POST parameter" + driver.firstName);
                Debug.WriteLine("POST parameter" + driver.lastName);
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
                    Debug.WriteLine("Driver not found");
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

        /// <summary>
        /// Deletes a driver from the database.
        /// </summary>
        /// <param name="id">The ID of the driver to delete.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the deletion operation.
        /// </returns>
        /// <example>
        /// POST: api/DriverData/DeleteDriver/5
        /// </example>

        [ResponseType(typeof(Driver))]
        [HttpPost]
        [Route("api/DriverData/DeleteDriver/{id}")]
        public IHttpActionResult DeleteDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            db.SaveChanges();

            return Ok();
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