using System.Text.Json;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    internal class ParsedHttpTelemetryProcessorInput {

        public string FunctionName { get; set; }

        public bool DiscardSuccessfulRequestTelemetry { get; set; }

        public bool FailureStatusCodeIsRequestFailure { get; set; }

        public static IEnumerable<ParsedHttpTelemetryProcessorInput> ParseFromSdkFunctionMetdata(
            IEnumerable<MinimalSdkFunctionMetadata> sdkFunctionMetadata ) {

            var parsedHttpTelemetryProcessorInputs = new List<ParsedHttpTelemetryProcessorInput>();

            foreach ( var functionMetadata in sdkFunctionMetadata ) {

                foreach ( var binding in functionMetadata.Bindings ) {

                    if ( binding["type"].ToString() != "httpTelemetryProcessor" ) {
                        continue;
                    }

                    var parsedInput = new ParsedHttpTelemetryProcessorInput() {
                        FunctionName = functionMetadata.Name
                    };

                    if ( binding.ContainsKey( "discardSuccessfulRequestTelemetry" ) ) {
                        parsedInput.DiscardSuccessfulRequestTelemetry = ( (JsonElement)binding["discardSuccessfulRequestTelemetry"] ).GetBoolean();
                    }

                    if ( binding.ContainsKey( "failureStatusCodeIsRequestFailure" ) ) {
                        parsedInput.FailureStatusCodeIsRequestFailure =
                            ( (JsonElement)binding["failureStatusCodeIsRequestFailure"] ).GetBoolean();
                    }

                    parsedHttpTelemetryProcessorInputs.Add( parsedInput );
                }
            }

            return parsedHttpTelemetryProcessorInputs;
        }

    }

}