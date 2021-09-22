using System;
using CommunityBox.Common.Kafka.Producer.Abstractions;
using CommunityBox.Common.Kafka.Producer.Configs;
using CommunityBox.Common.Kafka.Producer.Implementations;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CommunityBox.Common.Kafka.Producer
{
    public static class Entry
    {
        public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
            => serviceCollection.AddSingleton(typeof(IKafkaProducerMessageBus<,>), typeof(KafkaProducerMessageBus<,>));

        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services,
            Action<KafkaProducerConfig<TKey, TValue>> configAction)
        {
            services.AddConfluentKafkaProducer<TKey, TValue>();
            services.AddSingleton<KafkaProducer<TKey, TValue>>();

            services.Configure(configAction);

            return services;
        }

        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services,
            IConfigurationSection section)
        {
            services.AddKafkaMessageBus();
            services.AddConfluentKafkaProducer<TKey, TValue>();
            services.AddSingleton<KafkaProducer<TKey, TValue>>();

            services.Configure<KafkaProducerConfig<TKey, TValue>>(section);
            
            return services;
        }

        private static IServiceCollection AddConfluentKafkaProducer<TKey, TValue>(this IServiceCollection services)
        {
            services.AddSingleton(
                sp =>
                {
                    var config = sp.GetRequiredService<IOptions<KafkaProducerConfig<TKey, TValue>>>();

                    var builder =
                        new ProducerBuilder<TKey, TValue>(config.Value).SetValueSerializer(
                            new KafkaSerializer<TValue>());

                    return builder.Build();
                });

            return services;
        }
    }
}