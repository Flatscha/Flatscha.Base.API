namespace Flatscha.Base.API.ExampleAPI.Contracts.Constants
{
    public static class RouteConstants
    {
        public const string ROOT = "/api";

        public static class Example
        {
            public const string ROOT = "Example";

            public const string HELLO = "Hello/" + RouteParameter.Name;
        }

        public static class User
        {
            public const string ROOT = "User";

            public const string GET = "Get/" + RouteParameter.ID;
            public const string GET_ALL = "GetAll";
            public const string CREATE = "Create";
            public const string UPDATE = "Update/" + RouteParameter.ID;
            public const string DELETE = "Delete/" + RouteParameter.ID;
        }
    }
}
