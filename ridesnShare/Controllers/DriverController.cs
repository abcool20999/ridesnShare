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

        // POST: Driver/Create
        [HttpPost]
        public ActionResult Details(FormCollection collection)
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

        // GET: Driver/Edit/5
        public ActionResult Error(int id)
        {
            return View();
        }

        // POST: Driver/Edit/5
        [HttpPost]
        public ActionResult Add(int id, FormCollection collection)
        {
            {
                return View();
            }
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

        // POST: Driver/Delete/5
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
