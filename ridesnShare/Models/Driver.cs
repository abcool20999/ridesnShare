using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ridesnShare.Models
{
    public class Driver
    {
        //what describes a driver
        [Key]
        public int DriverId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        //a driver has many trips
        public ICollection<Trip> Trips { get; set; }
    }

    public class DriverDTO
    {
        public int DriverId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

    }
}