using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.CustomerDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;

namespace CustomerService.Messaging.RabbitMQ.Outbox
{
    public class Outbox
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _services;
        private readonly OutboxLogger logger;

        public Outbox(IBusClient busClient, ILogger<Outbox> logger, IServiceProvider services)
        {
            this.logger = new OutboxLogger(logger);
            try
            {
                _busClient = busClient;
                _services = services;
            }
            catch (Exception exp)
            {
                logger.LogError(exp, "Failed to initaite Outbox", null);
            }

        }


        public async Task PushPendingMessages()
        {
            var messagesToPush = FetchPendingMessages();
            logger.LogPending(messagesToPush);

            foreach (var msg in messagesToPush)
            {
                if (!await TryPush(msg))
                    break;
            }
        }

        private IList<Message> FetchPendingMessages()
        {
            List<Message> messagesToPush;

            using (var scope = _services.CreateScope())
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<CustomerContext>();

                messagesToPush = _context.Messages
               .Where(m => !m.IsProcced)
               .OrderBy(m => m.Id)
               .AsNoTracking()
               .Take(50)
               .ToList();

            }

            return messagesToPush;
        }

        private async Task<bool> TryPush(Message msg)
        {
            using (var scope = _services.CreateScope())
            {
                var _context =
                    scope.ServiceProvider
                        .GetRequiredService<CustomerContext>();
                using (var tx = _context.Database.BeginTransaction())
                {
                    try
                    {
                        await PublishMessage(msg);

                        msg.IsProcced = true;
                        msg.ProccesDateTime = DateTime.Now;

                        _context.Update(msg);

                        await _context.SaveChangesAsync();

                        tx.Commit();
                        logger.LogSuccessPush();
                        return true;
                    }
                    catch (Exception e)
                    {
                        logger.LogFailedPush(e);
                        tx?.Rollback();
                        return false;
                    }
                }
            }
        }

        private async Task PublishMessage(Message msg)
        {
            var deserializedMsg = msg.RecreateMessage();
            var messageKey = deserializedMsg.GetType().Name.ToLower();

            try
            {
                await _busClient.PublishAsync(deserializedMsg, msg.PublicId,
                            cfg =>
                            {
                                cfg.WithExchange(
                                    excfg =>
                                    {
                                        excfg.WithDurability(true);
                                        excfg.WithName("resilient-outbox-netcore");
                                    })
                                .WithRoutingKey(messageKey);
                            });
            }
            catch (Exception exp)
            {
                logger.LogFailedPush(exp);
            }
        }

    }

    class OutboxLogger
    {
        private readonly ILogger<Outbox> logger;

        public OutboxLogger(ILogger<Outbox> logger) => this.logger = logger;

        public void LogPending(IEnumerable<Message> messages)
        {
            logger.LogInformation($"{messages.Count()} messages about to be pushed.");
        }

        public void LogSuccessPush()
        {
            logger.LogInformation("Successfully pushed message");
        }

        public void LogFailedPush(Exception e)
        {
            logger.LogError(e, "Failed to push message from outbox", null);
        }
    }
}