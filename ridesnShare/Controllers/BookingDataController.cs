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
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of bookings from the database.
        /// </summary>
        /// <returns>
        /// An IEnumerable of BookingDTO objects representing the list of bookings.
        /// </returns>
        /// <example>
        /// GET: api/BookingData/ListBookings
        /// </example>
        [HttpGet]
        [Route("api/BookingData/ListBookings")]
        public IEnumerable<BookingDTO> ListBookings()
        {
            List<Trip> trips = db.Trips.Include(t =>t.Driver).Include(d =>d.Bookings).ToList();
            List<BookingDTO> BookingDTOs = new List<BookingDTO>();
            foreach (Trip trip in trips)
            {
                foreach (Booking booking in trip.Bookings)
                {
                    var passenger = db.Passengers.FirstOrDefault(p =>p.passengerId == booking.PassengerId);
                    BookingDTOs.Add(new BookingDTO()
                    {
                        passengerFirstName = passenger.firstName,
                        driverFirstName = trip.Driver.firstName,
                        startLocation = trip.startLocation,
                        endLocation = trip.endLocation,
                        price = trip.price,
                        Time = trip.Time,
                        dayOftheweek = trip.dayOftheweek
                    });
                }
            }

            return BookingDTOs;

        }
        /// <summary>
        /// Retrieves information about a specific booking from the database.
        /// </summary>
        /// <param name="id">The ID of the booking to retrieve.</param>
        /// <returns>
        /// An IHttpActionResult containing information about the booking.
        /// </returns>
        /// <example>
        /// GET: api/BookingData/FindBooking/{id}
        /// </example>

        [ResponseType(typeof(Booking))]
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

        // PUT: api/BookingData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.bookingId)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/BookingData
        [ResponseType(typeof(Booking))]
        public IHttpActionResult PostBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = booking.bookingId }, booking);
        }

        // DELETE: api/BookingData/5
        [ResponseType(typeof(Booking))]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok(booking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.bookingId == id) > 0;
        }
    }
}