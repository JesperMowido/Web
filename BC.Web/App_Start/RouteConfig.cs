using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BC.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            "House",
            "house/{placeslug}/{id}/{slug}",
             new { controller = "Product", action = "Index", placeslug = UrlParameter.Optional, slug = UrlParameter.Optional },
             new { id = @"\d+" }
             );

            routes.MapRoute(
            "Hus",
            "hus/{placeslug}/{id}/{slug}",
             new { controller = "Product", action = "Index", placeslug = UrlParameter.Optional, slug = UrlParameter.Optional },
             new { id = @"\d+" }
             );

            routes.MapRoute(
              "SokPlats",
              "bostader/{id}/{slug}",
               new { controller = "Search", action = "ListSearchPlace", slug = UrlParameter.Optional },
               new { id = @"\d+" }
               );

            routes.MapRoute(
              "SearchPlaces",
              "houses/{id}/{slug}",
               new { controller = "Search", action = "ListSearchPlace", slug = UrlParameter.Optional },
               new { id = @"\d+" }
               );

            routes.MapRoute(
             "SokKarta",
             "karta/{id}/{slug}",
              new { controller = "Search", action = "SearchMap", slug = UrlParameter.Optional },
              new { id = @"\d+" }
              );

            routes.MapRoute(
              "SearchMap",
              "map/{id}/{slug}",
               new { controller = "Search", action = "SearchMap", slug = UrlParameter.Optional },
               new { id = @"\d+" }
               );

            routes.MapRoute(
              "SokAllt",
              "bostader/alla",
               new { controller = "Search", action = "SearchAll", slug = UrlParameter.Optional },
               new { id = @"\d+" }
               );

            routes.MapRoute(
              "Searchall",
              "houses/all",
               new { controller = "Search", action = "SearchAll", slug = UrlParameter.Optional },
               new { id = @"\d+" }
               );

            routes.MapRoute(
              "HittaBostader",
              "bostader",
               new { controller = "Search", action = "ListAllPlaces" });

            routes.MapRoute(
              "FindHouses",
              "houses",
               new { controller = "Search", action = "ListAllPlaces" });

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "BuyingHome",
                url: "buy-home/{place}",
                defaults: new { controller = "Home", action = "BuyingHomeGuide", place = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "KopaBostad",
                url: "kopa-bostad/{place}",
                defaults: new { controller = "Home", action = "BuyingHomeGuide", place = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "OmOss",
                url: "om-oss",
                defaults: new { controller = "Home", action = "About" }
            );

            routes.MapRoute(
                name: "Logga in",
                url: "logga-in",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
