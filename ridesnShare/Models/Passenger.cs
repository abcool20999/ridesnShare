using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ridesnShare.Models;

namespace ridesnShare.Models
{
    public class Passenger
    {
        //what describes a passenger
        [Key]
        public int passengerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        //a passenger has many bookings
        public ICollection<Booking> Bookings { get; set; }

    }
}