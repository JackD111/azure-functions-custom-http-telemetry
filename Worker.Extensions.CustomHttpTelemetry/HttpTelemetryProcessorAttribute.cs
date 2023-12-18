using Microsoft.Azure.Functions.Worker.Extensions.Abstractions;

namespace Worker.Extensions.HttpTelemetryProcessor {

    public class HttpTelemetryProcessorAttribute : InputBindingAttribute {

        public bool DiscardSuccessfulRequestTelemetry { get; set; }

        public bool FailureStatusCodeIsRequestFailure { get; set; }


    }

}