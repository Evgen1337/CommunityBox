using System;
using CommunityBox.Common.Kafka.Consumer.Abstractions;
using CommunityBox.Common.Kafka.Consumer.Configs;
using CommunityBox.Common.Kafka.Consumer.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityBox.Common.Kafka.Consumer
{
    public static class Entry
    {
        public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(this IServiceCollection services,
            Action<KafkaConsumerConfig<TKey, TValue>> configAction) where THandler : class, IKafkaHandler<TKey, TValue>
        {
            services.AddScoped<IKafkaHandler<TKey, TValue>, THandler>();

            services.AddHostedService<KafkaConsumerBackgroundService<TKey, TValue>>();

            services.Configure(configAction);

            return services;
        }
        
        public static IServiceCollection AddKafkaConsumer<TKey, TValue, THandler>(this IServiceCollection services,
            IConfigurationSection section) where THandler : class, IKafkaHandler<TKey, TValue>
        {
            services.AddScoped<IKafkaHandler<TKey, TValue>, THandler>();

            services.AddHostedService<KafkaConsumerBackgroundService<TKey, TValue>>();

            services.Configure<KafkaConsumerConfig<TKey, TValue>>(section);

            return services;
        }
    }
}