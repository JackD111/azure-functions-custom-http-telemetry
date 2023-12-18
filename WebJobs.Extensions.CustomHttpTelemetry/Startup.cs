using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebJobs.Extensions.HttpTelemetryProcessor {

    public class Startup : IWebJobsStartup2 {
        
        public void Configure( IWebJobsBuilder builder ) {
            // wont be called
        }

        public void Configure( WebJobsBuilderContext context, IWebJobsBuilder builder ) {

            builder.AddExtension<HttpTelemetryProcessorExtensionConfigProvider>();

            var services = builder.Services;
            var configDescriptor = builder.Services.SingleOrDefault( sd => sd.ServiceType == typeof( TelemetryConfiguration ) );
            if ( configDescriptor?.ImplementationFactory == null ) {
                ConsoleLogger.Error( "No telemetry configuration found" );
                return;
            }

            var sdkFunctionMetaData = FunctionMetadataJsonReader.ReadMetadata( context.ApplicationRootPath );
            if ( sdkFunctionMetaData == null ) {
                ConsoleLogger.Error( "No function metadata found" );
                return;
            }

            var parsedHttpTelemetryProcessorInputs = ParsedHttpTelemetryProcessorInput.ParseFromSdkFunctionMetdata( sdkFunctionMetaData );

            var implementationFactory = configDescriptor.ImplementationFactory;
            services.Remove( configDescriptor );
            services.AddSingleton( provider => {
                if ( implementationFactory.Invoke( provider ) is not TelemetryConfiguration config ) {
                    ConsoleLogger.Error( "Factory invocation didn't return TelemetryConfiguration" );
                    return null;
                }

                config.TelemetryProcessorChainBuilder.Use( next => new HttpTelemetryProcessor( next, parsedHttpTelemetryProcessorInputs ) );
                config.TelemetryProcessorChainBuilder.Build();

                return config;
            } );
        }

    }

}