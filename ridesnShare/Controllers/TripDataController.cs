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
            TripDTO tripDTO = new TripDTO()
            {
                tripId = trip.tripId,
                startLocation = trip.startLocation,
                endLocation = trip.endLocation,
                price = trip.price,
                Time = trip.Time,
                dayOftheweek= trip.dayOftheweek
            };

            if (trip == null)
            {
                return NotFound();
            }

            return Ok(tripDTO);
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
        /// Adds a new trip to the database.
        /// </summary>
        /// <param name="trip">The trip object containing information about the new trip.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the addition operation.
        /// </returns>
        /// <example>
        /// POST: api/TripData/AddTrip/5
        /// </example>
        [ResponseType(typeof(Trip))]
        [HttpPost]
        public IHttpActionResult AddTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

       
            Driver driver = db.Drivers.Find(trip.DriverId);
            
            if( driver == null)
            {
                Debug.WriteLine("Driver doesn't exist");

                return BadRequest();
            }


            db.Trips.Add(trip);
            db.SaveChanges();

            return Ok("Driver added");
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