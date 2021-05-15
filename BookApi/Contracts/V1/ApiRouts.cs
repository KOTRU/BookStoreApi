namespace BookApi.Contracts.V1
{
    public static class ApiRouts
    {
        public const string Rout = "api";

        public const string Version = "v1";

        public const string Base = Rout + "/" + Version;

        public static class Genres
        {
            public const string GetAll = Base + "/genres";
            public const string Create = Base + "/genres";
            public const string Get = Base + "/genres/{genreId}";
            public const string Delete = Base + "/genres/{genreId}";
        }
        public static class Authors
        {
            public const string GetAll = Base + "/authors";
            public const string Create = Base + "/authors";
            public const string Get = Base + "/authors/{authorId}";
            public const string Delete = Base + "/authors/{authorId}";
        }
    }
}