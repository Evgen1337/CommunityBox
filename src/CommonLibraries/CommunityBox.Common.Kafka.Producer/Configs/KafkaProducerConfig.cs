using Confluent.Kafka;

namespace CommunityBox.Common.Kafka.Producer.Configs
{
    public class KafkaProducerConfig<TKey, TValue> : ProducerConfig
    {
        public string Topic { get; set; }
    }
}