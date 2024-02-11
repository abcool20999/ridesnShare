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
            //objective is to communicate with my Trip data api to retrieve one trip.
            //curl https://localhost:44354/api/TripData/FindTrip/id


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "FindTrip/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables me see if my httpclient is communicating with the data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an object of type trip.
            TripDTO selectedtrip = response.Content.ReadAsAsync<TripDTO>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("trip received");
            Debug.WriteLine(selectedtrip.price);
            //this shows the channel of comm btwn the webserver in my trip controller and the actual trip data controller api as we are communicating through an http request

            return View(selectedtrip);
        }

        // GET: Trip/Create
        public ActionResult Error()
        {
            return View();
        }

        // POST: Trip/Add       
        public ActionResult Add()
        {
           
            return View();
          
        }

        // POST: Trip/AddTrip
        [HttpPost]
        public ActionResult AddTrip(Trip trip)
        {
            Debug.WriteLine("the inputted trip name is :");
            Debug.WriteLine(trip.price);
            //objective: add a new trip into our system using the API
            //curl -H "Content-Type:application/json" -d @trip.json  https://localhost:44354/api/TripData/AddTrip
            string url = "AddTrip";

            //convert trip object into a json format to then send to our api
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(trip);

            Debug.WriteLine(jsonpayload);

            //send the json payload to the url through the use of our client
            //setup the postdata as HttpContent variable content
            HttpContent content = new StringContent(jsonpayload);

            //configure a header for our client to specify the content type of app for post 
            content.Headers.ContentType.MediaType = "application/json";

            //check if you can access information from our postasync request, get an httpresponse request and result of the request

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Errors");
            }
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

        // POST: Trip/Edit/5
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
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
        public ActionResult DeleteConfirm(int id)
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
