using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Backoffice
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowerCase(
                name:     "Login",
                url:      "login",
                defaults: new { controller = "authentication", action = "login", id = UrlParameter.Optional }
            );

            routes.MapRouteLowerCase(
                name:     "Default",
                url:      "{controller}/{action}/{id}",
                area:     "dashboard",
                defaults: new { controller = "dashboard", action = "index", id = UrlParameter.Optional }
            );
        }
    }

    public class LowercaseRoute : System.Web.Routing.Route
    {
        public LowercaseRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler) { }
        public LowercaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler) { }
        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler) { }
        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler) { }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            VirtualPathData path = base.GetVirtualPath(requestContext, values);
            if (path != null)
                path.VirtualPath = path.VirtualPath.ToLowerInvariant();
            return path;
        }
    }

    public static class RouteCollectionExtension
    {
        // Lower-case routes
        public static Route MapRouteLowerCase(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapRouteLowerCase(name, url, defaults, null);
        }
        public static Route MapRouteLowerCase(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints)
            };
            routes.Add(name, route);
            return route;
        }
        public static Route MapRouteLowerCase(this RouteCollection routes, string name, string url, string area, object defaults)
        {
            return routes.MapRouteLowerCase(name, url, area, defaults, null);
        }
        public static Route MapRouteLowerCase(this RouteCollection routes, string name, string url, string area, object defaults, object constraints)
        {
            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(new { area = area })
            };
            routes.Add(name, route);
            return route;
        }

        // Lower-case routes with areas
        public static Route MapRouteLowerCase(this AreaRegistrationContext context, string name, string url, object defaults)
        {
            return context.MapRouteLowerCase(name, url, defaults, null);
        }
        public static Route MapRouteLowerCase(this AreaRegistrationContext context, string name, string url, object defaults, object constraints)
        {
            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                DataTokens = new RouteValueDictionary(new { area = context.AreaName }),
                Constraints = new RouteValueDictionary(constraints)
            };
            context.Routes.Add(name, route);
            return route;
        }
        public static Route MapRouteLowerCase(this AreaRegistrationContext context, string name, string url, string area, object defaults)
        {
            return context.MapRouteLowerCase(name, url, area, defaults, null);
        }
        public static Route MapRouteLowerCase(this AreaRegistrationContext context, string name, string url, string area, object defaults, object constraints)
        {
            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                DataTokens = new RouteValueDictionary(new { area = area }),
                Constraints = new RouteValueDictionary(constraints)
            };
            context.Routes.Add(name, route);
            return route;
        }
    }
}