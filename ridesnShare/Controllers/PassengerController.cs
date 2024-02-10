using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using ridesnShare.Models;
using System.Web.Script.Serialization;
using System.Net.NetworkInformation;
using System.Data.Entity.Migrations.Model;



namespace ridesnShare.Controllers
{
    public class PassengerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PassengerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/api/PassengerData/");
        }
        // GET: Passenger/List
        public ActionResult List()
        {
            //objective is to communicate with my Passenger data api to retrieve a list of passengers.
            //curl https://localhost:44354/api/PassengerData/ListPassengers


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "ListPassengers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables us see if our httpclient is communicating with our data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an IEnumerable of type passenger.
            IEnumerable<PassengerDTO> passengers = response.Content.ReadAsAsync<IEnumerable<PassengerDTO>>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("Number of passengers received");
            Debug.WriteLine(passengers.Count());
            //this shows the channel of comm btwn our webserver in our passenger controller and the actual passenger data controller api as we are communicating through an http request

            return View(passengers);
        }

        // GET: Passenger/Details/5
        public ActionResult Details(int id)
        {
            //objective is to communicate with my Passenger data api to retrieve one passenger.
            //curl https://localhost:44354/api/PassengerData/FindPassenger/id


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "FindPassenger/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables see if our httpclient is communicating with our data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an object of type passenger.
            PassengerDTO selectedpassenger = response.Content.ReadAsAsync<PassengerDTO>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("passenger received");
            Debug.WriteLine(selectedpassenger.firstName);
            //this shows the channel of comm btwn our webserver in our passenger controller and the actual passenger data controller api as we are communicating through an http request

            return View(selectedpassenger);
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        // POST: Passenger/AddUser
        [HttpPost]
        public ActionResult AddUser(Passenger passenger)
        {
            Debug.WriteLine("the inputted passenger name is :");
            Debug.WriteLine(passenger.firstName);
            //objective: add a new passenger into our system using the API
            //curl -H "Content-Type:application/json" -d @passenger.json  https://localhost:44354/api/PassengerData/AddPassenger
            string url = "addpassenger";

            //convert passenger object into a json format to then send to our api
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(passenger);

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

        // GET: Passenger/Edit/5
        public ActionResult Edit(int id)
        {
            // Construct the URL to find the passenger with the given ID
            string url = "FindPassenger/" + id;

            // Send a GET request to retrieve the passenger information from the API
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the response content into a PassengerDTO object
            PassengerDTO selectedpassenger = response.Content.ReadAsAsync<PassengerDTO>().Result;

            // Return the View with the selected passenger data
            return View(selectedpassenger);
        }

        // POST: Passenger/Update/5
        [HttpPost]
        public ActionResult Update(int id, Passenger passenger)
        {
            // Set the passenger ID to match the ID in the route
            passenger.passengerId = id;

            // Construct the URL to update the passenger with the given ID
            string url = "UpdatePassenger/" + id;

            // Serialize the passenger object into JSON payload
            string jsonpayload = jss.Serialize(passenger);

            // Create HTTP content with JSON payload
            HttpContent content = new StringContent(jsonpayload);

            // Set the content type of the HTTP request to JSON
            content.Headers.ContentType.MediaType = "application/json";

            // Send a POST request to update the passenger information
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


        // GET: Passenger/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindPassenger/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PassengerDTO selectedpassenger = response.Content.ReadAsAsync<PassengerDTO>().Result;
            return View(selectedpassenger); 
            
        }

        // POST: Passenger/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeletePassenger/" + id;
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
