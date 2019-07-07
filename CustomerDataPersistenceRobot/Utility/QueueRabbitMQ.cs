using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerDataPersistenceRobot.Utility
{
    class QueueRabbitMQ
    {
        /*private Int32 QueueCount()
        {
            var response = model.QueueDeclarePassive("queue-name");
            response.MessageCount;
        
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
                {
                    var body = ea.Body;
                    // ... process the message
                    channel.BasicAck(ea.DeliveryTag, false);
                };
            String consumerTag = channel.BasicConsume(queueName, false, consumer);
            
        }*/

        public static void QueueConsume()
        {
            // Post the customer data to Queue / RabbitMQ Server
            var factory = new ConnectionFactory()
            {
                HostName = "whale-01.rmq.cloudamqp.com",
                UserName = "yyvswksf",
                VirtualHost = "yyvswksf",
                Password = "aw1hZCbj52fN4U--M3yX9NBSjAMx7xLS"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "Customer",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        // JsonConvert.DeserializeObject(message);

                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    channel.BasicConsume(
                        queue: "Customer",
                        autoAck: true,
                        consumer: consumer
                    );

                    Console.WriteLine("Queue Consuming... Press [enter] to exit");
                    Console.ReadLine();
                }
            }
        }

        private void QueuePost(string jsonCustomerNavigation)
        {
            // Post the customer data to Queue / RabbitMQ Server
            var factory = new ConnectionFactory()
            {
                HostName = "whale-01.rmq.cloudamqp.com",
                UserName = "yyvswksf",
                VirtualHost = "yyvswksf",
                Password = "aw1hZCbj52fN4U--M3yX9NBSjAMx7xLS"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "Customer",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var body = Encoding.UTF8.GetBytes(jsonCustomerNavigation);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "Customer",
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }
    }
}
