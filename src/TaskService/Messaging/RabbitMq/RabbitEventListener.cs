using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;

namespace TaskService.Messaging.RabbitMq
{
    public class RabbitEventListener
    {
        private IBusClient _busClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMemoryCache _cache;

        public RabbitEventListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    _busClient =
                        scope.ServiceProvider
                            .GetRequiredService<IBusClient>();

                    _cache = scope.ServiceProvider
                            .GetRequiredService<IMemoryCache>();
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        public void ListenTo(List<Type> eventsToSubscribe)
        {
            foreach (var evtType in eventsToSubscribe)
            {
                bool exist = false;
                _cache.TryGetValue(evtType.ToString(), out exist);
                if (!exist)
                {
                    this.GetType()
                        .GetMethod("Subscribe", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .MakeGenericMethod(evtType)
                        .Invoke(this, new object[] { });

                    _cache.Set(evtType.ToString(), true, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(3000)
                    });
                }

            }
        }

        private void Subscribe<T>() where T : INotification
        {
            if (_busClient != null)
            {
                SubscribeCore<T>();
            }
            else
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        _busClient =
                            scope.ServiceProvider
                                .GetRequiredService<IBusClient>();
                    }
                    SubscribeCore<T>();
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
        }

        private void SubscribeCore<T>() where T : INotification
        {
            _busClient.SubscribeAsync<T>(
                    async (msg, msgcontext) =>
                    {
                        //add logging
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var internalBus = scope.ServiceProvider.GetRequiredService<IMediator>();
                            await internalBus.Publish(msg);
                        }

                    },
                    cfg =>
                        cfg.WithExchange(excfg =>
                        {
                            excfg.WithName("resilient-outbox-netcore");
                            excfg.WithType(RawRabbit.Configuration.Exchange.ExchangeType.Topic);
                            excfg.WithArgument("key", typeof(T).Name.ToLower());
                        })
                        .WithQueue(qcfg =>
                        {
                            qcfg.WithName("task-service-" + typeof(T).Name);
                        })
                    );
        }

    }
}
