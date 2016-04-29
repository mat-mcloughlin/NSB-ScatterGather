using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Persistence;
using Raven.Client.Embedded;

//using Raven.Client.Document;
//using Raven.Client.Embedded;

namespace Monitor
{
    class Program
    {
        static void Main()
        {
            #region setup
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Scatter-Gather.Monitor");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();

            var persistence = busConfiguration.UsePersistence<RavenDBPersistence>();
            persistence.DoNotSetupDatabasePermissions(); //Only required to simplify the sample setup
            persistence.SetDefaultDocumentStore(SetupDocumentStore());

            //var persistence = busConfiguration.UsePersistence<NHibernatePersistence>();
            //persistence.ConnectionString(@"Data Source=.;Initial Catalog=ScatterGather;Integrated Security=True");

            #endregion setup

            using (var bus = Bus.Create(busConfiguration).Start())
            {
                var fileId = Guid.NewGuid();
                bus.SendLocal(new NewZipFile
                {
                    FileId = fileId,
                    ZipPath = @"c:\temp\codepo_gb.zip",
                    ExtractPath = @"c:\temp\processed\"
                });

                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
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
