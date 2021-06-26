using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace CustomerService.Messaging.RabbitMQ.Outbox
{
    public class OutboxSendingService : IHostedService
    {
        private readonly Outbox outbox; 
        private static SemaphoreSlim semaphoreSlim;

        private Timer timer;
        private static readonly object locker = new object();

        public OutboxSendingService(Outbox outbox)
        {
            this.outbox = outbox;
            semaphoreSlim = new SemaphoreSlim(1, 1);
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer
            (
                PushMessages,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1)
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

                await outbox.PushPendingMessages();

            }
            finally
            {
                semaphoreSlim.Release();
            }
        }
    }
}