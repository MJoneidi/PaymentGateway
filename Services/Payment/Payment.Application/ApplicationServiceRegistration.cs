﻿using AutoMapper;
using Gateway.Business.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Payment.Application.BankAdaptors;
using Payment.Application.Commands;
using Payment.Application.Commands.Contracts;
using Payment.Application.Contracts;
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

            services.AddScoped<ICommandHandler<PaymentCommand>, PaymentCommandHandler>();
            return services;
        }
    }
}
