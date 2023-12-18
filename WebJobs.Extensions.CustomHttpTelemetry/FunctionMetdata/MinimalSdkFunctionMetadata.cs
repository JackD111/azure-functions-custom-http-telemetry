namespace WebJobs.Extensions.HttpTelemetryProcessor {

    internal class MinimalSdkFunctionMetadata {

        public string? Name { get; set; }

        public List<IDictionary<string, object>> Bindings { get; set; } = new();

    }

}