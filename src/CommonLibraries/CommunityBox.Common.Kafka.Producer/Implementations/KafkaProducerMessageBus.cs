using System.Threading.Tasks;
using CommunityBox.Common.Kafka.Producer.Abstractions;
using Confluent.Kafka;

namespace CommunityBox.Common.Kafka.Producer.Implementations
{
    public class KafkaProducerMessageBus<TKey, TValue> : IKafkaProducerMessageBus<TKey, TValue>
    {
        private readonly KafkaProducer<TKey, TValue> _producer;

        public KafkaProducerMessageBus(KafkaProducer<TKey, TValue> producer)
        {
            _producer = producer;
        }

        public async Task PublishAsync(Message<TKey, TValue> message)
        {
            await _producer.ProduceAsync(message);
        }
    }
}