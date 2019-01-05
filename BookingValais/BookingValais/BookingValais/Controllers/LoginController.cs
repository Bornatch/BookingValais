using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO;

namespace BookingValais.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(string password)
        {
            Client client = null;
            
            string surname = Request["surname"];
            string name = Request["name"];

            client = ClientManager.GetClient(surname, name, password);
            if(client == null)
            {
                ViewData["Error"] = "The user is unknown.";
                return View("Login");
            }

            else
            {
                Session["IdClient"] = client;
                return View("LoginOk");
            }
                

            //return View("LoginOk");
        }

        public ActionResult Logout()
        {
            Session["IdClient"] = null;
            return View("LogoutOk");
        }
    }
}