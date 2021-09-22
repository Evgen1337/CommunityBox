using Confluent.Kafka;

namespace CommunityBox.Common.Kafka.Consumer.Configs
{
    public class KafkaConsumerConfig<TKey, TValue> : ConsumerConfig
    {
        public string Topic { get; set; }
    }
}