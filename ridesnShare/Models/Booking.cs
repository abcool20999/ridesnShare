﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ridesnShare.Models
{
    public class Booking
    {

        //what describes a booking
        [Key]
        public int bookingId { get; set; }
       

        //a booking has a passenger ID
        //a passenger has many bookings
        [ForeignKey("Passenger")]
        public int PassengerId { get; set; }
        public virtual Passenger Passenger { get; set; }

        //a booking has a trip ID
        //a trip has many bookings

        [ForeignKey("Trip")]
        public int tripId { get; set; }
        public virtual Trip Trip { get; set; }

    }

    public class BookingDTO
    {
        public int bookingId { get; set; }
        public int PassengerId { get; set; }
        public int tripId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int Age { get; set; }
        public string CarType { get; set; }


    }
}


