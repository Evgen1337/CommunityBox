using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityBox.Common.Kafka.Consumer.Abstractions;
using CommunityBox.Common.Kafka.Consumer.Configs;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace CommunityBox.Common.Kafka.Consumer.Implementations
{
    public class KafkaConsumerBackgroundService<TKey, TValue> : BackgroundService
    {
        private readonly KafkaConsumerConfig<TKey, TValue> _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumerBackgroundService(IOptions<KafkaConsumerConfig<TKey, TValue>> config,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            await Task.Factory.StartNew(async s =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IKafkaHandler<TKey, TValue>>();

                while (true)
                {
                    try
                    {
                        await ConsumingTopicAsync(handler, ct);
                    }
                    catch (Exception)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(30), ct);
                    }
                }
            }, ct, TaskCreationOptions.LongRunning);
        }

        private async Task ConsumingTopicAsync(IKafkaHandler<TKey, TValue> handler, CancellationToken ct)
        {
            var builder =
                new ConsumerBuilder<TKey, TValue>(_config)
                    .SetValueDeserializer(new KafkaDeserializer<TValue>());

            using var consumer = builder.Build();
            consumer.Subscribe(_config.Topic);


            while (!ct.IsCancellationRequested)
            {
                var result = consumer.Consume(ct);

                if (result == null)
                    continue;

                await handler.HandleAsync(result.Message);

                consumer.Commit(result);
                consumer.StoreOffset(result);
            }
        }
    }
}