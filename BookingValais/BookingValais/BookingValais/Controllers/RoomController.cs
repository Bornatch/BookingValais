﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace BookingValais.Controllers
{
    public class RoomController : Controller
    {
        // GET: Room
        public ActionResult Index(int idHotel, DateTime dateStart, DateTime dateEnd)
        {
            string dateStartText = dateStart.ToString("dd-MM-yyyy");
            string dateEndText = dateEnd.ToString("dd-MM-yyyy");
            ViewData["Hotel"] = HotelManager.GetHotel(idHotel);
            ViewData["ListRooms"] = RoomManager.GetRooms(idHotel, dateStartText, dateEndText);
            ViewData["Pictures"] = PictureManager.GetPicturesURL(idHotel);
            ViewData["dateStart"] = dateStart;
            ViewData["dateEnd"] = dateEnd;
            return View("Room");
        }

        

        public int GetNumberValue(string value)
        {
            switch (value) //returns int values based on string content in dropdownlists
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
                case "Six":
                    return 6;
                case "Seven":
                    return 7;
                case "Eight":
                    return 8;
                case "Nine":
                    return 9;
            }
            return 0;
        }
    }
}