using Flatscha.Base.API.ExampleAPI.Contracts.Constants;
using System;

namespace Flatscha.Base.API.ExampleAPI.Contracts.Extensions
{
    public static class RouteExtensions
    {
        public static string SetRouteParameterID(this string route, Guid id) => route.Replace(RouteParameter.ID, id.ToString());
        public static string SetRouteParameterID(this string route, int id) => route.Replace(RouteParameter.ID, id.ToString());
        public static string SetRouteParameterName(this string route, string value) => route.Replace(RouteParameter.Name, value);
        public static string SetRouteParameterNumber(this string route, string value) => route.Replace(RouteParameter.Number, value);
    }
}
