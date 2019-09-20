using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OopRestaurant201909
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //TZ: Atirtuk a controller-t home-rol, ami a project letrehozasakor legeneralt oldalt tolti be
                defaults: new { controller = "MenuItems", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
