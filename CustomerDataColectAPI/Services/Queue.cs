using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDataColectAPI.Services
{
    // manage queue
    public class Queue
    {
        public void QueuePostJson(String jsonCustomerNavigation, String Queue)
        {
            // pass to ConnectionStrings in appsettings.json
            var factory = new ConnectionFactory()
            {
                HostName = "whale-01.rmq.cloudamqp.com",
                UserName = "yyvswksf",
                VirtualHost = "yyvswksf",
                Password = "aw1hZCbj52fN4U--M3yX9NBSjAMx7xLS"
            };

            // connect to RabbitMQ
            using (var connection = factory.CreateConnection())
            {
                // Open channel
                using (var channel = connection.CreateModel())
                {
                    // Declare
                    channel.QueueDeclare(
                        queue: Queue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    // Prepare String in bytes
                    var body = Encoding.UTF8.GetBytes(jsonCustomerNavigation);

                    // send to queue
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: Queue,
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }

    }
}
