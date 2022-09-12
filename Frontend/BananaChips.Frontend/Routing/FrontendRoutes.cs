using Flurl;

namespace BananaChips.Frontend.Routing;

public static class FrontendRoutes
{
    public static string GetRouteWithParameter(string route, object parameterValue)
    {
        return string.Join('/', route.Split('/').SkipLast(1)).AppendPathSegment(parameterValue.ToString());
    }
    
    public const string Login = "/login";
    public const string Dashboard = "/";

    public static class Companies
    {
        private const string Prefix = Dashboard + "companies";
        public const string List = Prefix;
        public const string New = Prefix + "/add";
        public const string Edit = Prefix + "/edit/{Id:int}";
    }

    public static class Invoices
    {
        private const string Prefix = Dashboard + "invoices";
        public const string List = Prefix;
        public const string New = Prefix + "/add";
    }
}