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

        /// <summary>Connection string to the Users database.</summary>
        public static string UsersDbConnectionString =>
            $"User ID={User};Password={Password};Host={Host};Port={Port};Database={DatabaseName};";
    }

    /// <summary>Constants related to Kafka.</summary>
    public static class Kafka
    {
        // Retrieve Kafka service host from environment variable "KAFKA_SERVICE_HOST"
        private static readonly string KafkaServiceHost =
            Environment.GetEnvironmentVariable("KAFKA_SERVICE_HOST")!;

        // Retrieve Kafka service port from environment variable "KAFKA_SERVICE_PORT_HTTP"
        private static readonly string KafkaServicePort =
            Environment.GetEnvironmentVariable("KAFKA_SERVICE_PORT_HTTP")!;
        
        /// <summary>
        /// Concatenate host and port to create Kafka connection string
        /// </summary>
        public static readonly string KafkaConnectionString = $"{KafkaServiceHost}:{KafkaServicePort}";
    }

}