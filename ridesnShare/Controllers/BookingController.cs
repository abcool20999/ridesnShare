using ridesnShare.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ridesnShare.Controllers
{
    public class BookingController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookingController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/api/BookingData/");
        }
        // GET: Booking/List
        public ActionResult List()
        {
            //objective is to communicate with my Booking data api to retrieve a list of passengers.
            //curl https://localhost:44354/api/BookingData/ListBookings


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "ListBookings";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables us see if our httpclient is communicating with our data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an IEnumerable of type passenger.
            IEnumerable<BookingDTO> bookings = response.Content.ReadAsAsync<IEnumerable<BookingDTO>>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("Number of passengers received");
            Debug.WriteLine(bookings.Count());
            //this shows the channel of comm btwn our webserver in our passenger controller and the actual passenger data controller api as we are communicating through an http request

            return View(bookings);
        }


        // GET: Booking/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
