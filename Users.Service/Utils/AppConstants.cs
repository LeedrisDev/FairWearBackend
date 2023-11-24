namespace Users.Service.Utils;

/// <summary>Class to hold constants for the application.</summary>
public static class AppConstants
{
    /// <summary>Constant related to the databases.</summary>
    public static class Database
    {
        private static readonly string Host = Environment.GetEnvironmentVariable("USERS_DB_SERVICE_HOST")!;
        private static readonly string Port = Environment.GetEnvironmentVariable("USERS_DB_SERVICE_PORT_HTTP")!;
        private static readonly string User = Environment.GetEnvironmentVariable("USERS_DB_USER")!;
        private static readonly string Password = Environment.GetEnvironmentVariable("USERS_DB_PASSWORD")!;
        private static readonly string DatabaseName = Environment.GetEnvironmentVariable("USERS_DB_DATABASE_NAME")!;

        /// <summary>Connection string to the Brand and Product database.</summary>
        public static string BrandAndProductConnectionString =>
            $"User ID={User};Password={Password};Host={Host};Port={Port};Database={DatabaseName};";
    }
}