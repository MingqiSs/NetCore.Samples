using System;
using System.Threading;
namespace KafkaProducerDome
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
            var producer = new KafkaProducer();
            for (int i = 0; i < 100; i++)
            {
                var msgData = Newtonsoft.Json.JsonConvert.SerializeObject(new Msg { name = "zs", text = $"第 {i+1} 条消息" });
                producer.SendKafkaMsg( "test_consumer_topic", msgData);
                Console.WriteLine($"send {msgData}");
                Thread.Sleep(10000);
            }
        
        }
    }
}
