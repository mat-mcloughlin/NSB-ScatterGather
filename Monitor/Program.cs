using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using Raven.Client.Document;

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

            var documentStore = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "ScatterGather"
            };
            documentStore.Initialize();

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
