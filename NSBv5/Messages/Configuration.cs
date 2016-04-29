using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace Messages
{
    public class ConfigurationSource : IConfigurationSource
    {
        public T GetConfiguration<T>() where T : class, new()
        {
            if (typeof(T) == typeof(MessageForwardingInCaseOfFaultConfig))
            {
                MessageForwardingInCaseOfFaultConfig errorConfig = new MessageForwardingInCaseOfFaultConfig
                {
                    ErrorQueue = "error"
                };

                return errorConfig as T;
            }

            return ConfigurationManager.GetSection(typeof(T).Name) as T;
        }
    }
}
