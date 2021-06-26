using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomerService.Messaging.RabbitMQ.Outbox
{
    public class OutboxSendingService : IHostedService
    {
        private Outbox _outbox;
        private static SemaphoreSlim semaphoreSlim;

        private Timer timer;
        private static readonly object locker = new object();
        private readonly IServiceProvider _services;
        private readonly ILogger<OutboxSendingService> _logger;

        public OutboxSendingService(IServiceProvider services, ILogger<OutboxSendingService> logger)
        {
            _services = services;
            _logger = logger;
            semaphoreSlim = new SemaphoreSlim(1, 1);

            try
            {
                using (var scope = _services.CreateScope())
                {
                    _outbox =
                        scope.ServiceProvider
                            .GetRequiredService<Outbox>();
                }
            }
            catch (Exception exp)
            {
                _logger.LogError(exp, "OutboxSendingService init");
            }
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer
            (
                PushMessages,
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


        private async void PushMessages(object state)
        {

            try
            {
                await semaphoreSlim.WaitAsync();

                if (_outbox != null)
                    await _outbox.PushPendingMessages();
                else
                {
                    try
                    {
                        using (var scope = _services.CreateScope())
                        {
                            _outbox =
                                scope.ServiceProvider
                                    .GetRequiredService<Outbox>();
                        }
                    }
                    catch (Exception exp)
                    {
                        _logger.LogError(exp, "OutboxSendingService init");
                    }
                }

            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}