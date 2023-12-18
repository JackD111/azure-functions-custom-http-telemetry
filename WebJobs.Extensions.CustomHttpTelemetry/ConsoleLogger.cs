using System.Globalization;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    internal static class ConsoleLogger {

        private const string LOG_PREFIX = "HttpTelemetryProcessor:";

        public static void Information( string message ) {
            Console.WriteLine( $"[{GetTimestamp()}] {LOG_PREFIX} {message}" );
        }

        public static void Error( string message ) {
            Console.Error.WriteLine( $"[{GetTimestamp()}] {LOG_PREFIX} {message}" );
        }

        private static string GetTimestamp() {
            return DateTime.UtcNow.ToString( "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture );
        }
    }

}