using System;
using BookingValais.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace BookingValais.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Home()
        {
            //reinitializes error status
            ViewData["Error"] = "";
            return View();
        }

        public ActionResult About()
        {
            return View("About");
        }

        public ActionResult Hotel()
        {
            return View("Hotel");
        }

        [Route("api/Hotels/{action}/{dateStart}/{dateEnd}/{location}/{persons}")]
        public ActionResult GetHotels()
        {
            Console.WriteLine("Enter try");
            try //check if all fields have data            
            {
                DateTime today = DateTime.Now;
                //simple search
                int persons = GetNumberValue(Request.Form.GetValues("HotelPersons")[0]);
                DateTime dateStart = Convert.ToDateTime(Request["txtDateStart"].ToString());
                DateTime dateEnd = Convert.ToDateTime(Request["txtDateEnd"].ToString());
                string location = Convert.ToString(Request["txtLocation"].ToString());

                String dateStartText = "";

                dateStartText = dateStart.ToString("dd-MM-yyyy");

                String dateEndText = "";
                dateEndText = dateEnd.ToString("dd-MM-yyyy");

                if(persons == 0 || location == "")
                {
                    ViewData["Error"] = "Please fill all fields";
                    return View("Home");
                }
                Console.WriteLine("person pass");

                if (dateStart < today)
                {
                    ViewData["Error"] = "Your check-in must be at least tomorrow.";
                    return View("Home");
                }
                Console.WriteLine("today check pass");

                if (dateEnd < dateStart)
                {
                    ViewData["Error"] = "Your check-out must be after your check-in";
                    return View("Home");
                }

                Console.WriteLine("diff date pass");

                //advanced search
                Boolean parking = Convert.ToBoolean(Request.Form.GetValues("checkParking")[0]);
                Boolean wifi = Convert.ToBoolean(Request.Form.GetValues("checkWifi")[0]);
                int stars = GetNumberValue(Request.Form.GetValues("HotelStars")[0]);
                Boolean hasTV = Convert.ToBoolean(Request.Form.GetValues("checkTV")[0]);
                Boolean hasHairDryer = Convert.ToBoolean(Request.Form.GetValues("checkHairDryer")[0]);

                //check if advanced search criteria has been selected or not
                if (!parking && !wifi && !hasTV && !hasHairDryer && stars == 0)
                {
                    ViewData["ListHotels"] = HotelManager.GetAvailableHotels(dateStartText, dateEndText, location, persons);
                }                
                    
                else
                {
                    ViewData["ListHotels"] = HotelManager.GetAvailableHotelsAdvanced(dateStartText, dateEndText, location, persons, Request.Form.GetValues("checkWifi")[0],
                        Request.Form.GetValues("checkParking")[0], stars, Request.Form.GetValues("checkTV")[0], Request.Form.GetValues("checkHairDryer")[0]);
                }
                    

                ViewData["DateStart"] = dateStart;
                ViewData["DateEnd"] = dateEnd;

                //create IEnumerable variable to count and check if search yields any results
                IEnumerable<DTO.Hotel> listHotel = ViewData["ListHotels"] as IEnumerable<DTO.Hotel>;
                
                if (listHotel.Count() == 0) //if no results
                {
                    ViewData["Error"] = "No hotels were found with these parameters.";
                    return View("Home");
                }
                else
                {
                    //get pictures
                    List<string> pictureUrl = new List<string>();
                    foreach(DTO.Hotel hotel in listHotel)
                    {
                        pictureUrl.AddRange(PictureManager.GetPicturesHotel(hotel.IdHotel));
                    }
 
                    ViewData["Pictures"] = pictureUrl;

                    return View("Hotel");
                }
            }

            catch(Exception e)
            {                
                ViewData["Error"] = "Please fill all fields !";
                return View("Home");
                throw e;
            }
            
        }

        public int GetNumberValue(string value)
        {
            switch(value) //returns int values based on string content in dropdownlists
            {
                case "One":
                    return 1;
                case "Two":
                    return 2;
                case "Three":
                    return 3;
                case "Four":
                    return 4;
                case "Five":
                    return 5;
            }
            return 0;
        }


    }
}