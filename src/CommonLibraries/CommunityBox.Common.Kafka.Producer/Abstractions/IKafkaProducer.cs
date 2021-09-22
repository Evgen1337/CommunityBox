using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace CommunityBox.Common.Kafka.Producer.Abstractions
{
    public interface IKafkaProducer<TKey, TValue> : IDisposable
    {
        Task ProduceAsync(Message<TKey, TValue> message, CancellationToken ct = default);

        Task ProduceAsync(Message<TKey, TValue> message, TopicPartition topicPartition, CancellationToken ct = default);
    }
}