using System.Threading;
using System.Threading.Tasks;
using CommunityBox.Common.Kafka.Producer.Abstractions;
using CommunityBox.Common.Kafka.Producer.Configs;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace CommunityBox.Common.Kafka.Producer.Implementations
{
    public class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly KafkaProducerConfig<TKey, TValue> _topicConfig;
        private readonly IProducer<TKey, TValue> _producer;

        public KafkaProducer(IOptions<KafkaProducerConfig<TKey, TValue>> topicConfig, IProducer<TKey, TValue> producer)
        {
            _producer = producer;
            _topicConfig = topicConfig.Value;
        }

        public async Task ProduceAsync(Message<TKey, TValue> message, CancellationToken ct = default)
        {
            await _producer.ProduceAsync(_topicConfig.Topic, message, ct);
        }

        public async Task ProduceAsync(Message<TKey, TValue> message, TopicPartition topicPartition,
            CancellationToken ct = default)
        {
            await _producer.ProduceAsync(topicPartition, message, ct);
        }

        public void Dispose() => _producer.Dispose();
    }
}