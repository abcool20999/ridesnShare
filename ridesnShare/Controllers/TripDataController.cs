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
    public class TripDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Retrieves a list of trips from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of TripDTO objects representing the list of trips.
        /// </returns>
        /// <example>
        /// GET: api/TripData/ListTrips
        /// </example>
        [HttpGet]
        [Route("api/TripData/ListTrips")]
        public IEnumerable<TripDTO> Trips()
        {
            List<Trip> Trips = db.Trips.ToList();
            List<TripDTO> TripDTOs = new List<TripDTO>();

            Trips.ForEach(t => TripDTOs.Add(new TripDTO()
            {
                tripId = t.tripId,
                startLocation = t.startLocation,
                endLocation = t.endLocation,
                price = t.price,
                Time = t.Time,
                dayOftheweek = t.dayOftheweek
            }));

            return TripDTOs;

        }

        /// <summary>
        /// Retrieves information about a specific trip from the database.
        /// </summary>
        /// <param name="id">The ID of the trip to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing information about the trip.
        /// </returns>
        /// <example>
        /// GET: api/TripData/FindTrip/{id}
        /// </example>

        [ResponseType(typeof(Trip))]
        [HttpGet]
        [Route("api/TripData/FindTrip/{id}")]
        public IHttpActionResult FindTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            TripDTO passengerDTO = new PassengerDTO()
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

        // PUT: api/TripData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateTrip(int id, Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trip.tripId)
            {
                return BadRequest();
            }

            db.Entry(trip).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripExists(id))
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

        // DELETE: api/TripData/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult DeleteTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            db.Trips.Remove(trip);
            db.SaveChanges();

            return Ok(trip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TripExists(int id)
        {
            return db.Trips.Count(e => e.tripId == id) > 0;
        }
    }
}