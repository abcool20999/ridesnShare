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
                        bookingId = booking.bookingId,
                        passengerFirstName = booking.Passenger.firstName,
                        driverFirstName = booking.Trip.Driver.firstName,
                        startLocation = booking.Trip.startLocation,
                        endLocation = booking.Trip.endLocation,
                        price = booking.Trip.price,
                        Time = booking.Trip.Time,
                        dayOftheweek = booking.Trip.dayOftheweek
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
        [Route("api/BookingData/FindBooking/{id}")]
        public IHttpActionResult FindBooking(int id)
        {
            Booking booking = db.Bookings.Include(b=>b.Trip).FirstOrDefault(b=>b.bookingId==id);
            BookingDTO bookingDTO = new BookingDTO()
            {
                bookingId = booking.bookingId,
                PassengerId = booking.PassengerId,
                DriverId = booking.Trip.DriverId,
                tripId = booking.tripId,
                passengerFirstName = booking.Passenger.firstName,
                driverFirstName = booking.Trip.Driver.firstName,
                startLocation = booking.Trip.startLocation,
                endLocation = booking.Trip.endLocation,
                price = booking.Trip.price,
                Time = booking.Trip.Time,
                dayOftheweek = booking.Trip.dayOftheweek
            };

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(bookingDTO);
        }

        /// <summary>
        /// Updates information about a specific passengerbooking in the database.
        /// </summary>
        /// <param name="id">The ID of the passengerbooking to update.</param>
        /// <param name="passengerbooking">The updated information of the passengerbooking.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the update operation.
        /// </returns>
        /// <example>
        /// POST: api/BookingData/UpdateBooking/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Route("api/BookingData/UpdateBooking/{id}")]
        public IHttpActionResult UpdateBooking(int id, BookingDTO bookingDTO)
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
                Debug.WriteLine("POST parameter" + bookingDTO.Time);
                Debug.WriteLine("POST parameter" + bookingDTO.dayOftheweek);
                Debug.WriteLine("POST parameter" + bookingDTO.endLocation);
                return BadRequest();
            }
            var passenger = db.Passengers.FirstOrDefault(x => x.passengerId==bookingDTO.PassengerId);
            var driver = db.Drivers.FirstOrDefault(x => x.DriverId == bookingDTO.DriverId);
            var trip = db.Trips.FirstOrDefault(x => x.tripId == bookingDTO.tripId);
            var booking = db.Bookings.FirstOrDefault(x => x.bookingId == bookingDTO.bookingId);

            passenger.firstName=bookingDTO.passengerFirstName;
            driver.firstName=bookingDTO.driverFirstName;
            trip.startLocation=bookingDTO.startLocation;
            trip.endLocation=bookingDTO.endLocation;
            trip.price=bookingDTO.price;
            trip.Time=bookingDTO.Time;
            trip.dayOftheweek=bookingDTO.dayOftheweek;

            db.Entry(passenger).State = EntityState.Modified;
            db.Entry(driver).State = EntityState.Modified;
            db.Entry(trip).State = EntityState.Modified;
            db.Entry(booking).State = EntityState.Modified;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    Debug.WriteLine("Booking not found");
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

        /// <summary>
        /// Deletes a booking from the database.
        /// </summary>
        /// <param name="id">The ID of the booking to delete.</param>
        /// <returns>
        /// An IHttpActionResult indicating the result of the deletion operation.
        /// </returns>
        /// <example>
        /// POST: api/BookingData/DeleteBooking/5
        /// </example>

        [ResponseType(typeof(Booking))]
        [HttpPost]
        [Route("api/BookingData/DeleteBooking/{id}")]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
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

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.bookingId == id) > 0;
        }
    }
}