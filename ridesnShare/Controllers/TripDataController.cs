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

        // GET: api/TripData
        public IQueryable<Trip> GetTrips()
        {
            return db.Trips;
        }

        // GET: api/TripData/5
        [ResponseType(typeof(Trip))]
        public IHttpActionResult GetTrip(int id)
        {
            Trip trip = db.Trips.Find(id);
            if (trip == null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        // PUT: api/TripData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrip(int id, Trip trip)
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

        // POST: api/TripData
        [ResponseType(typeof(Trip))]
        public IHttpActionResult PostTrip(Trip trip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trips.Add(trip);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trip.tripId }, trip);
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