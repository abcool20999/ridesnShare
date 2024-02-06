using ridesnShare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace ridesnShare.Controllers
{
    public class PassengerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all passengers in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all passengers in the database.
        /// </returns>
        /// <example>
        /// GET: api/PassengerData/ListPassengers
        /// </example>
        [HttpGet]
        [Route("api/PassengerData/ListPassengers")]
        [ResponseType(typeof(Passenger))]
        public IHttpActionResult ListPassengers()
        {
            List<Passenger> Passengers = db.Passengers.ToList();
            
            return Ok(Passengers);
        }

    }
}
