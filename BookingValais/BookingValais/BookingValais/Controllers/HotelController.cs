using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO;
using BLL;
using System.Web.Mvc;

namespace BookingValais.Controllers
{
    public class HotelController : Controller
    {
        // GET: Hotel
        public ActionResult Index()
        {
            List<Hotel> all = HotelManager.GetHotels();
            all.ToList();
            return View(all);
        }
    }
}