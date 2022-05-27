using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducerDome
{
    public  class KafkaProducer
    {
        private IProducer<Null, string> _producer = null;
        public IProducer<Null, string> GetProducerBuilder()
        {
            if (_producer == null)
            {
                var conf = new ProducerConfig
                {
                    BootstrapServers = "bl-kafka-001:9092",
                    MessageMaxBytes = 100000000
                };
                _producer = new ProducerBuilder<Null, string>(conf).Build();
                return _producer;
            }
            else
            {
                return _producer;
            }
        }
        /// <summary>
        /// kafka发送消息
        /// </summary>
        /// <param name="strTopic">kafka的topic</param>
        /// <param name="strJsonData">消息主体</param>
        public async void SendKafkaMsg(
            string strTopic,
            string strJsonData)
        {
            string strLastRes = strJsonData;
            try
            {
                var p = GetProducerBuilder();
                var t = p.ProduceAsync(strTopic, new Message<Null, string> { Value = strLastRes });
                await t.ContinueWith(task =>
              {
                  if (task.IsFaulted)
                  {
                      Console.WriteLine($"Faulted");
                  }
                  else
                  {
                      Console.WriteLine($"Wrote to offset: {task.Result.Offset}");
                  }
              });
                p.Flush(TimeSpan.FromSeconds(10));
            }
            catch (Exception exception)
            {
                Console.WriteLine("发送失败" + exception.ToString());
            }


        }
    }
}
