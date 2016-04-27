using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Persistence;
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
            #region setup
            var endpointConfiguration = new EndpointConfiguration("Scatter-Gather.Monitor");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            //var persistence = endpointConfiguration.UsePersistence<RavenDBPersistence>();
            //persistence.DoNotSetupDatabasePermissions(); //Only required to simplify the sample setup
            //persistence.SetDefaultDocumentStore(SetupDocumentStore());

            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.ConnectionString(@"Data Source=.;Initial Catalog=ScatterGather;Integrated Security=True");

            endpointConfiguration.UnicastRouting().RouteToEndpoint(typeof(ProcessFile), "Scatter-Gather.Processor");
            #endregion setup
            
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

        private static EmbeddableDocumentStore SetupDocumentStore()
        {
            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data",
                UseEmbeddedHttpServer = true,
                DefaultDatabase = "Scatter-Gather",
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
            return documentStore;
        }
    }
}
