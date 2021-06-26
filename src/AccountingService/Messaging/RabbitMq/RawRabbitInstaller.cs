using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Extensions.Client;
using IBusClient = RawRabbit.IBusClient;

namespace AccountingService.Messaging.RabbitMq
{
    public static class RawRabbitInstaller
    {
        public static IServiceCollection AddRabbitListeners(this IServiceCollection services
            , RabbitMqOptions options, IWebHostEnvironment env)
        {
            services.AddRawRabbit(
                cfg => cfg.AddJsonFile(
                    env.EnvironmentName == "Production" ? "rabbit.Production.json" : "rabbit.Development.json")
            );

            services.AddSingleton(svc => new RabbitEventListener(svc.GetRequiredService<IBusClient>(), svc));
            
            return services;
        }
    }

    public static class RabbitListenersInstaller
    {
        public static void UseRabbitListeners(this IApplicationBuilder app, List<Type> eventTypes)
        {
            app.ApplicationServices.GetRequiredService<RabbitEventListener>().ListenTo(eventTypes);
        }
    }
}
