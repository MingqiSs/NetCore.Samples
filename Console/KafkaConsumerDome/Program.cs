using Confluent.Kafka;
using System;

namespace KafkaConsumerDome
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-gorp",//组id带后缀v1/v2,
                FetchMinBytes = 300,
                EnableAutoCommit = true,
                AutoCommitIntervalMs = 5000,
                BootstrapServers = "bl-kafka-001:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest //AutoOffsetResetType.Earliest
            };
            var topic = "test_consumer_topic";
            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(topic);

                while (true)
                {
                    var result = c.Consume(TimeSpan.FromMilliseconds(100));
                    if (result != null)
                    {
                        var offset = result.Offset.Value;
                        var partition = result.Partition.Value;
                       var r_topic = result.Topic;
                        long unixTimestampMs =  result.Message.Timestamp.UnixTimestampMs;
                        Console.WriteLine($"Consume Value:{result.Message.Value},offset :{offset} " +
                            $",partition:{partition},r_topic:{r_topic},unixTimestampMs={unixTimestampMs}");
                    }
                }
            }
        }
    }
}
