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
    public class PassengerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves a list of passengers from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of PassengerDTO objects representing the list of passengers.
        /// </returns>
        /// <example>
        /// GET: api/PassengerData/ListPassengers
        /// </example>
        [HttpGet]
        [Route("api/PassengerData/ListPassengers")]
        public IEnumerable<PassengerDTO> ListPassengers()
        {
            List<Passenger> Passengers = db.Passengers.ToList();
            List<PassengerDTO> PassengerDTOs = new List<PassengerDTO>();

            Passengers.ForEach(p => PassengerDTOs.Add(new PassengerDTO()
            {
                passengerId = p.passengerId,
                firstName = p.firstName,
                lastName = p.lastName,
                email = p.email
            }));

            return PassengerDTOs;     

        }
        /// <summary>
        /// Retrieves information about a specific passenger from the database.
        /// </summary>
        /// <param name="id">The ID of the passenger to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing information about the passenger.
        /// </returns>
        /// <example>
        /// GET: api/PassengerData/FindPassenger/{id}
        /// </example>
      
        [ResponseType(typeof(Passenger))]
        [HttpGet]
        [Route("api/PassengerData/FindPassenger/{id}")]
        public IHttpActionResult FindPassenger(int id)
        {
            Passenger passenger = db.Passengers.Find(id);
            PassengerDTO passengerDTO = new PassengerDTO()
            {
                passengerId = passenger.passengerId,
                firstName = passenger.firstName,
                lastName = passenger.lastName,
                email = passenger.email
            };

            if (passenger == null)
            {
                return NotFound();
            }

            return Ok(passengerDTO);
        }
        /// <summary>
        /// Updates information about a specific passenger in the database.
        /// </summary>
        /// <param name="id">The ID of the passenger to update.</param>
        /// <param name="passenger">The updated information of the passenger.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the update operation.
        /// </returns>
        /// <example>
        /// POST: api/PassengerData/UpdatePassenger/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/PassengerData/UpdatePassenger/{id}")]
        public IHttpActionResult UpdatePassenger(int id, Passenger passenger)
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
                Debug.WriteLine("POST parameter" + passenger.passengerId);
                Debug.WriteLine("POST parameter" + passenger.firstName);
                Debug.WriteLine("POST parameter" + passenger.lastName);
                return BadRequest();
            }

            db.Entry(passenger).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(id))
                {
                    Debug.WriteLine("Passenger not found");
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
        /// Adds a new passenger to the database.
        /// </summary>
        /// <param name="passenger">The passenger object containing information about the new passenger.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the addition operation.
        /// </returns>
        /// <example>
        /// POST: api/PassengerData/AddPassenger/5
        /// </example>
        [ResponseType(typeof(Passenger))]
        [HttpPost]
        public IHttpActionResult AddPassenger(Passenger passenger)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Passengers.Add(passenger);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = passenger.passengerId }, passenger);
        }
        /// <summary>
        /// Deletes a passenger from the database.
        /// </summary>
        /// <param name="id">The ID of the passenger to delete.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the deletion operation.
        /// </returns>
        /// <example>
        /// POST: api/PassengerData/DeletePassenger/5
        /// </example>

        [ResponseType(typeof(Passenger))]
        [HttpPost]
        [Route("api/PassengerData/DeletePassenger/{id}")]
        public IHttpActionResult DeletePassenger(int id)
        {
            Passenger passenger = db.Passengers.Find(id);
            if (passenger == null)
            {
                return NotFound();
            }

            db.Passengers.Remove(passenger);
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

        private bool PassengerExists(int id)
        {
            return db.Passengers.Count(e => e.passengerId == id) > 0;
        }
    }
}