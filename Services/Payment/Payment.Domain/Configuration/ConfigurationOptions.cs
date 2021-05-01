using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Configuration
{
    public class ConfigurationOptions : IConfigurationOptions
    {
        public ConfigurationOptions(IConfiguration configuration)
        {
            this.ConnectionString = configuration.GetConnectionString("ReadConnection");
            this.Url = configuration["AcquiringBank:Url"];
        }
        public string ConnectionString { get; init; }
        public string Url { get; init; }
    }
}
