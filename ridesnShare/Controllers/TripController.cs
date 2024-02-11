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
    public class TripController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TripController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/api/TripData/");
        }
        // GET: Trip/List
        public ActionResult List()
        {
            //objective is to communicate with my Trip data api to retrieve a list of trips.
            //curl https://localhost:44354/api/TripData/ListTrips


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "ListTrips";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables us see if our httpclient is communicating with our data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an IEnumerable of type trip.
            IEnumerable<TripDTO> trips = response.Content.ReadAsAsync<IEnumerable<TripDTO>>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("Number of trips received");
            Debug.WriteLine(trips.Count());
            //this shows the channel of comm btwn the webserver, the trip controller and the actual trip data controller api as we are communicating through an http request

            return View(trips);
        }

        // GET: Trip/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Trip/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trip/Create
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

        // GET: Trip/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Trip/Edit/5
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

        // GET: Trip/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Trip/Delete/5
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
