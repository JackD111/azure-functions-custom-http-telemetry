using Microsoft.Azure.WebJobs.Description;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    [AttributeUsage( AttributeTargets.Parameter )]
    [Binding]
    public class HttpTelemetryProcessorAttribute : Attribute { }

}