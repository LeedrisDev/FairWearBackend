namespace BackOffice.Utils;

public abstract class AppConstants
{
    public static class Database
    {
        private static readonly string Host = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_SERVICE_HOST")!;
        private static readonly string Port = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_SERVICE_PORT_HTTP")!;
        private static readonly string User = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_USER")!;
        private static readonly string Password = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_PASSWORD")!;
        private static readonly string DatabaseName = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_DATABASE_NAME")!;
        
        public static string ConnectionString => $"User ID={User};Password={Password};Host={Host};Port={Port};Database={DatabaseName};";
    }
}