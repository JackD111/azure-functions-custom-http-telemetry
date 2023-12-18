using System.Text.Json;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    // Code taken from sdk\Sdk\WorkerNamingPolicy.cs
    internal class FunctionsJsonNamingPolicy : JsonNamingPolicy {

        public override string ConvertName( string name ) {
            // We need to camelCase everything but this one property or the host won't honor it.
            if ( string.Equals( "IsCodeless", name, System.StringComparison.OrdinalIgnoreCase ) ) {
                return "IsCodeless";
            }

            return CamelCase.ConvertName( name );
        }

    }

}