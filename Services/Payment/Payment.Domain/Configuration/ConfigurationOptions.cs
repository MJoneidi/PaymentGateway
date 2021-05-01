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
            this.GatewayPaymentId= new Guid(configuration["AcquiringBank:GatewayPaymentId"]);
        }
        public string ConnectionString { get; init; }
        public string Url { get; init; }


        /// <summary>
        /// this is unique id so the Acquiring Bank can recognize the sender
        /// </summary>
        public Guid GatewayPaymentId { get; init; }
    }
}
