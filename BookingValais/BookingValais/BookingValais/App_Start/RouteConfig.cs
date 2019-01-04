using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookingValais
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );

            routes.MapRoute ("Search", "api/{controller}/{action}/{dateStart}/{dateEnd}/{location}/{persons:int}",
                new
                {
                    dateStart = UrlParameter.Optional,
                    dateEnd = UrlParameter.Optional,
                    location = UrlParameter.Optional,
                    persons = UrlParameter.Optional
                });
            }
    }
}
