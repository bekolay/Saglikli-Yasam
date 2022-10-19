using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace yaglikli_yasa
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
            name: "sitemap",
            url: "sitemap.xml",
            defaults: new { controller = "sitemap", action = "Index" }

            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "d",
            url: "d/{id}",
            defaults: new { controller = "D", action = "Index", id = UrlParameter.Optional }

        );

            routes.MapRoute(
            name: "kategori",
            url: "kategori/{id}",
            defaults: new { controller = "Kategori", action = "Index", id = UrlParameter.Optional }

            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "yaglikli_yasa.Controllers" }
            );
        }
    }
}
