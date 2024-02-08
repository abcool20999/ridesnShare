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
    public class PassengerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PassengerData/ListPassengers
        [HttpGet]
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

        // GET: api/PassengerData/FindPassenger/{id}

        [ResponseType(typeof(Passenger))]
        [HttpGet]
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

        // POST: api/PassengerData/UpdatePassenger/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePassenger(int id, Passenger passenger)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != passenger.passengerId)
            {
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PassengerData/AddPassenger
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

        // DELETE: api/PassengerData/DeletePassenger/5
        [ResponseType(typeof(Passenger))]
        [HttpPost]
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