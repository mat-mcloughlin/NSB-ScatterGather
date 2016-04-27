using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Raven.Client.Document;
using Raven.Client.Embedded;

namespace Monitor
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            var endpointConfiguration = new EndpointConfiguration("Scatter-Gather.Monitor");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data",
                UseEmbeddedHttpServer = true,
                DefaultDatabase = "catter-Gather",
                Configuration =
                {
                    Port = 32076,
                    PluginsDirectory = Environment.CurrentDirectory,
                    HostName = "localhost"
                }
            };
            documentStore.Initialize();

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new DefaultTraceListener());

            var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.DoNotSetupDatabasePermissions(); //Only required to simplify the sample setup
            persistence.SetDefaultDocumentStore(documentStore);
            
            endpointConfiguration.UnicastRouting().RouteToEndpoint(typeof(ProcessFiles), "Scatter-Gather.Processor");

            var endpoint = await Endpoint.Start(endpointConfiguration);
            try
            {
                var fileId = Guid.NewGuid();
                Console.WriteLine("Press any key to process");
                Console.ReadKey();
                await endpoint.SendLocal(new NewZipFile { FileId = fileId, ZipPath = @"c:\temp\codepo_gb.zip", ExtractPath = @"c:\temp\processed\" });

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
            finally
            {
                await endpoint.Stop();
            }
        }
    }
}
