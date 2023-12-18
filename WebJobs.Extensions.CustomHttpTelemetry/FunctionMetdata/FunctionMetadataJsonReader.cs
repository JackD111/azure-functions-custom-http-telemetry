using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    internal class FunctionMetadataJsonReader {

        private const string FILE_NAME = "functions.metadata";
        private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

        private static JsonSerializerOptions CreateSerializerOptions() {
            var namingPolicy = new FunctionsJsonNamingPolicy();
            return new JsonSerializerOptions {
                WriteIndented = true,
                IgnoreNullValues = true,
                PropertyNameCaseInsensitive = true,
                IgnoreReadOnlyProperties = true,
                DictionaryKeyPolicy = namingPolicy,
                PropertyNamingPolicy = namingPolicy,
                Converters = {
                    new JsonStringEnumConverter(),
                }
            };
        }

        public static IEnumerable<MinimalSdkFunctionMetadata>? ReadMetadata( string metadataFileDirectory ) {
            var metadataFile = Path.Combine( metadataFileDirectory, FILE_NAME );

            if ( !File.Exists( metadataFile ) ) {
                return null;
            }

            var jsonBytes = File.ReadAllBytes( metadataFile );
            var reader = new Utf8JsonReader( jsonBytes );

            return JsonSerializer.Deserialize<IEnumerable<MinimalSdkFunctionMetadata>>( ref reader, SerializerOptions );
        }

    }

}