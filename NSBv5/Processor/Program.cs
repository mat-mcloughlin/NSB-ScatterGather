using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Persistence;

namespace Processor
{
    class Program
    {
        static void Main()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("Scatter-Gather.Process");
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();

            Bus.Create(busConfiguration).Start();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}