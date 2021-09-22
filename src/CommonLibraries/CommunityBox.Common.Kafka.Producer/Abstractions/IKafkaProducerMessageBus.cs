using System.Threading.Tasks;
using Confluent.Kafka;

namespace CommunityBox.Common.Kafka.Producer.Abstractions
{
    public interface IKafkaProducerMessageBus<TKey, TValue>
    {
        Task PublishAsync(Message<TKey, TValue> message);
    }
}