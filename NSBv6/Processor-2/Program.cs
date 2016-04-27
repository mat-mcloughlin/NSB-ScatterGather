using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Processor_2
{
    class Program
    {
        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            var endpointConfiguration = new EndpointConfiguration("Scatter-Gather.Processor");
            endpointConfiguration.ScaleOut().InstanceDiscriminator("Processor_2");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            
            await Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
