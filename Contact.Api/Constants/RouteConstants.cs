using System.Globalization;

namespace Contact.Api.Constants
{
    public class RouteConstants
    {
        public const string BASE_ROUTE = "api/v1";

        public const string ROUTE_CONTACT = BASE_ROUTE + "/contact";

        public const string ROUTE_CONTACT_BY_ID = ROUTE_CONTACT + "{contactId}";
    }
}
