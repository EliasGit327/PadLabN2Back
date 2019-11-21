using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using PadLabN2_BackEnd.Controllers;

namespace PAD_LabN2_BackEnd.Services.Consumer
{
    public class ConsumerService: IConsumerService
    {
        List<string> Messages = new List<string>();
        private MessageHub _messageHub;
        
        ConsumerConfig conf = new ConsumerConfig
        { 
            GroupId = "basic-consumer-group",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        public ConsumerService
        (
            MessageHub messageHub
        )
        {
            _messageHub = messageHub;
            Task.Run(async () => { StartConsume(Messages); });
        }

        public List<string> GetMessages()
        {
            return Messages;
        }

        public void StartConsume(List<string> messageList)
        {
            using var c = new ConsumerBuilder<Ignore, string>(conf).Build();
            c.Subscribe("my-replicated-topic");

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    try
                    {
                        var cr = c.Consume(cts.Token);
                        messageList.Add(cr.Message.Value);
                        _messageHub.AddMessage(cr.Message.Value);
                        Console.WriteLine($"Consumed message '{cr.Offset}' at: '{cr.TopicPartitionOffset}'.");
                    }
                    catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                c.Close();
            }
        } 
    }
}