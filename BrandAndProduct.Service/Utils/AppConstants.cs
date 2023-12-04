namespace BrandAndProduct.Service.Utils;

/// <summary>Class to hold constants for the application.</summary>
public static class AppConstants
{
    private static readonly string GoodOnYouScrapperServiceHost =
        Environment.GetEnvironmentVariable("GOODONYOU_SCRAPPER_SERVICE_HOST")!;

    private static readonly string GoodOnYouScrapperServicePortHttp =
        Environment.GetEnvironmentVariable("GOODONYOU_SCRAPPER_SERVICE_PORT_HTTP")!;

    private static readonly string ProductDataRetrieverServiceHost =
        Environment.GetEnvironmentVariable("PRODUCT_DATA_RETRIEVER_SERVICE_HOST")!;

    private static readonly string ProductDataRetrieverServicePortHttp =
        Environment.GetEnvironmentVariable("PRODUCT_DATA_RETRIEVER_SERVICE_PORT_HTTP")!;

    private static readonly string KafkaServiceHost =
        Environment.GetEnvironmentVariable("KAFKA_SERVICE_HOST")!;

    private static readonly string KafkaServicePort =
        Environment.GetEnvironmentVariable("KAFKA_SERVICE_PORT_HTTP")!;

    public static readonly string KafkaConnectionString = $"{KafkaServiceHost}:{KafkaServicePort}";

    /// <summary>URL for the Good On You Scrapper microservice.</summary>
    public static readonly string GoodOnYouScrapperUrl =
        $"http://{GoodOnYouScrapperServiceHost}:{GoodOnYouScrapperServicePortHttp}";

    /// <summary>URL for the Product Data Retriever microservice.</summary>
    public static readonly string ProductDataRetrieverUrl =
        $"http://{ProductDataRetrieverServiceHost}:{ProductDataRetrieverServicePortHttp}";

    /// <summary>Constant related to the databases.</summary>
    public static class Database
    {
        private static readonly string Host = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_SERVICE_HOST")!;

        private static readonly string Port =
            Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_SERVICE_PORT_HTTP")!;

        private static readonly string User = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_USER")!;
        private static readonly string Password = Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_PASSWORD")!;

        private static readonly string DatabaseName =
            Environment.GetEnvironmentVariable("BRAND_AND_PRODUCT_DB_DATABASE_NAME")!;

        /// <summary>Connection string to the Brand and Product database.</summary>
        public static string BrandAndProductConnectionString =>
            $"User ID={User};Password={Password};Host={Host};Port={Port};Database={DatabaseName};";
    }
}