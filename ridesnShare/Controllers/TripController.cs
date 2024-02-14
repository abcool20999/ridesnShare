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
        private readonly TripDataController _tripdatacontroller;

        public TripController()
        {
            _tripdatacontroller = new TripDataController();
        }
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

        // GET: Trip/Edit/5
        public ActionResult Edit(int id)
        {
            // Construct the URL to find the trip with the given ID
            string url = "FindTrip/" + id;

            // Send a GET request to retrieve the trip information from the API
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the response content into a TripDTO object
            TripDTO selectedtrip = response.Content.ReadAsAsync<TripDTO>().Result;

            // Return the View with the selected trip data
            return View(selectedtrip);
        }

        // POST: Trip/Update/5
        [HttpPost]
        public ActionResult Update(int id, Trip trip)
        {
            // Set the trip ID to match the ID in the route
            trip.tripId = id;

            Debug.WriteLine(trip.DriverId + "----- in update controller");

            // Construct the URL to update the trip with the given ID
            string url = "UpdateTrip/" + id;

            // Serialize the trip object into JSON payload
            string jsonpayload = jss.Serialize(trip);

            // Create HTTP content with JSON payload
            HttpContent content = new StringContent(jsonpayload);

            // Set the content type of the HTTP request to JSON
            content.Headers.ContentType.MediaType = "application/json";

            // Send a POST request to update the trip information
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // Log the content of the request
            Debug.WriteLine(content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Redirect to the List action if the update was successful
                return RedirectToAction("List");
            }
            else
            {
                // Redirect to the Error action if there was an error during the update
                return RedirectToAction("Error");
            }
        }

        public ActionResult SearchForTrip(int id)
        {
            ViewBag.passengerId = id;

            return View();
        }
        [HttpPost]
        public ActionResult SearchForTripPost(int id, string location, string destination)
        {
            var availabletrips = _tripdatacontroller.SearchForTrip(location, destination);

            if (availabletrips == default)
            {
                return View("AvailableTrips");
            }

            ViewBag.passengerId = id;

            return View("AvailableTrips", availabletrips);
        }

        // GET: Trip/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindTrip/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TripDTO selectedtrip = response.Content.ReadAsAsync<TripDTO>().Result;
            return View(selectedtrip);

        }

        // GET: Trip/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            string url = "DeleteTrip/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}