using AutoMapper;
using Gateway.Business.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.BankAdaptors;
using Payment.Application.BankAdaptors.Contracts;
using Payment.Application.Commands;
using Payment.Application.Commands.Contracts;
using Payment.Application.Processors;
using Payment.Application.Processors.Contracts;
using Payment.Application.Queries;
using Payment.Domain.Configuration;

namespace Payment.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPaymentQueries, PaymentQueries>();
            services.AddScoped<IAcquiringBankAdapter, AcquiringBankAdapter>();

            services.AddSingleton<IConfigurationOptions, ConfigurationOptions>();

            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {

                cfg.AddProfile(new MapperProfile());
            }).CreateMapper());

            services.AddScoped<ICommandHandler<CreatePaymentCommand>, PaymentCommandHandler>();
            services.AddScoped<IPaymentProcessor, PaymentProcessor>();
            return services;
        }
    }
}
