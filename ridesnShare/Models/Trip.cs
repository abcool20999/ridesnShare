using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ridesnShare.Models
{
    public class Trip
    {

        //what describes a trip
        [Key]
        public int tripId { get; set; }
        public string startLocation { get; set; }
        public string endLocation { get; set; }
        public string price { get; set; }
        public DateTime Time { get; set; }
        public string dayOftheweek { get; set; }

        //a trip has a driver ID
        //a driver has many trips
        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }


        //a driver has many trips
        public ICollection<Booking> Bookings { get; set; }
    }

    public class TripDTO
    {
        public int tripId { get; set; }
        public string startLocation { get; set; }
        public string endLocation { get; set; }
        public string price { get; set; }

        public int DriverId { get; set; }
        public DateTime Time { get; set; }

        public string dayOftheweek { get;set; }

    }
}
