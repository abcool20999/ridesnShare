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
    public class DriverController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DriverController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44354/api/DriverData/");
        }
        // GET: Driver/List
        public ActionResult List()
        {
            //objective is to communicate with my Driver data api to retrieve a list of drivers.
            //curl https://localhost:44354/api/DriverData/ListDrivers


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "ListDrivers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables us see if our httpclient is communicating with our data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an IEnumerable of type driver.
            IEnumerable<DriverDTO> drivers = response.Content.ReadAsAsync<IEnumerable<DriverDTO>>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("Number of drivers received");
            Debug.WriteLine(drivers.Count());
            //this shows the channel of comm btwn our webserver in our driver controller and the actual driver data controller api as we are communicating through an http request

            return View(drivers);
        }

        // GET: Driver/Details/5
        public ActionResult Details(int id)
        {
            //objective is to communicate with my Driver data api to retrieve one driver.
            //curl https://localhost:44354/api/DriverData/FindDriver/id


            //Establish url connection endpoint i.e client sends info and anticipates a response
            string url = "FindDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //this enables me see if my httpclient is communicating with the data access endpoint 

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            //objective is to parse the content of the response message into an object of type driver.
            DriverDTO selecteddriver = response.Content.ReadAsAsync<DriverDTO>().Result;

            //we use debug.writeline to test and see if its working
            Debug.WriteLine("driver received");
            Debug.WriteLine(selecteddriver.firstName);
            //this shows the channel of comm btwn our webserver in my driver controller and the actual driver data controller api as we are communicating through an http request

            return View(selecteddriver);
        }

        // GET: Driver/Error/5
        public ActionResult Error(int id)
        {
            return View();
        }

        // POST: Driver/Add
    
        public ActionResult Add()
        {
            
          return View();
            
        }

        // POST: Driver/AddUser
        [HttpPost]
        public ActionResult AddUser(Driver driver)
        {
            Debug.WriteLine("the inputted driver name is :");
            Debug.WriteLine(driver.firstName);
            //objective: add a new driver into our system using the API
            //curl -H "Content-Type:application/json" -d @driver.json  https://localhost:44354/api/DriverData/AddDriver
            string url = "addDriver";

            //convert driver object into a json format to then send to our api
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(driver);

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

        // GET: Driver/Edit/5
        public ActionResult Edit(int id)
        {
            // Construct the URL to find the driver with the given ID
            string url = "FindDriver/" + id;

            // Send a GET request to retrieve the driver information from the API
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Deserialize the response content into a DriverDTO object
            DriverDTO selecteddriver = response.Content.ReadAsAsync<DriverDTO>().Result;

            // Return the View with the selected driver data
            return View(selecteddriver);
        }

        // POST: Driver/Update/5
        [HttpPost]
        public ActionResult Update(int id, Driver driver)
        {
            // Set the driver ID to match the ID in the route
            driver.DriverId = id;

            // Construct the URL to update the driver with the given ID
            string url = "UpdateDriver/" + id;

            // Serialize the driver object into JSON payload
            string jsonpayload = jss.Serialize(driver);

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


        // GET: Driver/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DriverDTO selecteddriver = response.Content.ReadAsAsync<DriverDTO>().Result;
            return View(selecteddriver);

        }

        // POST: Driver/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DeleteDriver/" + id;
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
