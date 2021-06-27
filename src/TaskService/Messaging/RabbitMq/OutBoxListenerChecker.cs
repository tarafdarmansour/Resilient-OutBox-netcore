using CustomerService.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskService.Messaging.RabbitMq
{
    public class OutBoxListenerChecker : IHostedService
    {
        private readonly IServiceProvider _services;
        private RabbitEventListener _rabbitEventListener;
        private readonly ILogger<OutBoxListenerChecker> _logger;
        private IBusClient _busClient;
        private static SemaphoreSlim semaphoreSlim;
        private Timer timer;

        public OutBoxListenerChecker(IServiceProvider services, ILogger<OutBoxListenerChecker> logger)
        {
            _services = services;
            _logger = logger;
            semaphoreSlim = new SemaphoreSlim(1, 1);
            using (var scope = _services.CreateScope())
            {
                try
                {
                    _busClient =
                        scope.ServiceProvider
                            .GetRequiredService<IBusClient>();

                    _rabbitEventListener =
                        scope.ServiceProvider
                            .GetRequiredService<RabbitEventListener>();
                }
                catch (Exception exp)
                {
                }


            }

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer
            (
                SubscribeListener,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5)
            );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        private async void SubscribeListener(object state)
        {
            try
            {
                await semaphoreSlim.WaitAsync();

                using (var scope = _services.CreateScope())
                {
                    try
                    {
                        _busClient =
                            scope.ServiceProvider
                                .GetRequiredService<IBusClient>();

                        _rabbitEventListener =
                            scope.ServiceProvider
                                .GetRequiredService<RabbitEventListener>();
                    }
                    catch (Exception exp)
                    {
                    }

                }

                if (_busClient != null)
                {
                    _rabbitEventListener.ListenTo(new List<Type>
                    {
                        typeof(CustomerCreated),
                        typeof(CustomerUpdated),
                        typeof(CustomerDeleted),
                    });
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }

        }
    }
}
