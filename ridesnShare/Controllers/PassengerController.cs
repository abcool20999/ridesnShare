using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using ridesnShare.Models;

namespace ridesnShare.Controllers
{
    public class PassengerController : Controller
    {
        // GET: Passenger/List
        public ActionResult List()
        {
            //objective is to communicate with my Passenger data api to retrieve a list of passengers.
            //curl https://localhost:44354/api/PassengerData/ListPassengers

            HttpClient client = new HttpClient() { };
            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = " https://localhost:44354/api/PassengerData/ListPassengers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables see if our httpclient is communicating with our data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an IEnumerable of type passenger.
            IEnumerable<Passenger> passengers = response.Content.ReadAsAsync<IEnumerable<Passenger>>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("Number of animals received");
            Debug.WriteLine(passengers.Count());
            //this shows the channel of comm btwn our webserver in our animal controller and the actual animal data controller api as we are communicating through an http request

            return View(passengers);
        }

        // GET: Passenger/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Passenger/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Passenger/Create
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

        // GET: Passenger/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Passenger/Edit/5
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

        // GET: Passenger/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Passenger/Delete/5
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
