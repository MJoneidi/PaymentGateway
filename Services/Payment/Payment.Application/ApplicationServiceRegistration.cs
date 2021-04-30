using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application
{
    public static class ApplicationServiceRegistration
    {        
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =  configuration.GetConnectionString("ReadConnection") ;
            services.AddScoped<IPaymentQueries, PaymentQueries>(rec=> new PaymentQueries(connectionString));          

            return services;
        }
    }
}
