using System;
using Confluent.Kafka;
using PAD_LabN2_BackEnd.Models;

namespace PAD_LabN2_BackEnd.Services
{
    public class ProducerService: IProducerService
    {
        private ProducerConfig _producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };

        public ProducerService() {}
        
        Action<DeliveryReport<Null, string>> handler = r =>
        {
            Console.WriteLine(!r.Error.IsError ? $"Delivered message to {r.TopicPartitionOffset}" 
                : $"Delivery Error: {r.Error.Reason}");
        };
        
        public bool ProduceMessage(Message message)
        {
            try
            {
                using (var producer = new ProducerBuilder<Null, string>((_producerConfig)).Build())
                {
                    producer.Produce(message.TopicName, 
                        new Message<Null, string> { Value = message.Body }, handler);
                    producer.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}