using System;
using NServiceBus;

namespace Processor_2
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