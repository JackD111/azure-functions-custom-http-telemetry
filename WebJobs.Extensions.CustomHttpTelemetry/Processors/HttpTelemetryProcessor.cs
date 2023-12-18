using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    internal class HttpTelemetryProcessor : ITelemetryProcessor {

        private readonly IReadOnlyDictionary<string, ParsedHttpTelemetryProcessorInput> parsedHttpTelemetryProcessorInputsByFunction;

        private readonly ITelemetryProcessor next;

        public HttpTelemetryProcessor( ITelemetryProcessor next, IEnumerable<ParsedHttpTelemetryProcessorInput> parsedHttpTelemetryProcessorInputs ) {
            this.next = next;

            parsedHttpTelemetryProcessorInputsByFunction = parsedHttpTelemetryProcessorInputs
                .ToDictionary(
                    x => $"Functions.{x.FunctionName}",
                    x => x,
                    StringComparer.OrdinalIgnoreCase );

            foreach ( var pair in parsedHttpTelemetryProcessorInputsByFunction ) {
                ConsoleLogger.Information( $"{pair.Key}: DiscardSuccessfulRequestTelemetry={pair.Value.DiscardSuccessfulRequestTelemetry}, FailureStatusCodeIsRequestFailure={pair.Value.FailureStatusCodeIsRequestFailure}" );
            }
        }

        public void Process( ITelemetry item ) {

            var skipProcessing = false;
            try {

                if ( item is not RequestTelemetry requestTelemetry ) {
                    return;
                }

                if ( !int.TryParse( requestTelemetry.ResponseCode, out var responseCode )
                     || !requestTelemetry.Properties.TryGetValue( "FullName", out var fullName ) || fullName == null
                     || !parsedHttpTelemetryProcessorInputsByFunction.TryGetValue( fullName, out var parsedHttpTelemetryProcessorInput ) ) {
                    return;
                }

                if ( parsedHttpTelemetryProcessorInput.DiscardSuccessfulRequestTelemetry && responseCode < 400 ) {
                    ConsoleLogger.Information( $"{fullName} was successful, discarding RequestTelemetry." );
                    skipProcessing = true;
                    return;
                }

                if ( parsedHttpTelemetryProcessorInput.FailureStatusCodeIsRequestFailure && responseCode >= 400 ) {
                    ConsoleLogger.Information( $"{fullName} failed with status code {responseCode}." );
                    requestTelemetry.Success = false;
                }

            } finally {

                if ( !skipProcessing ) {
                    next.Process( item );
                }
            }

        }

    }

}