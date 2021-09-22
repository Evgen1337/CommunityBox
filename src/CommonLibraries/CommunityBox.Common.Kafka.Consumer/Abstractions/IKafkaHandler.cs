using System.Threading.Tasks;
using Confluent.Kafka;

namespace CommunityBox.Common.Kafka.Consumer.Abstractions
{
    public interface IKafkaHandler<TKey, TValue>
    {
        Task HandleAsync(Message<TKey, TValue> message);
    }
}