using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    [Extension( "HttpTelemetryProcessor" )]
    internal class HttpTelemetryProcessorExtensionConfigProvider : IExtensionConfigProvider {

        public void Initialize( ExtensionConfigContext context ) {
            var bindingRule = context.AddBindingRule<HttpTelemetryProcessorAttribute>();
            bindingRule.BindToInput<string?>( attr => "" );
        }

    }

}