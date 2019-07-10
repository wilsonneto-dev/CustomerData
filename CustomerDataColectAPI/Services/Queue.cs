using CustomerDataColectAPI.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private readonly AppSettings AppSettings;
        public Queue (IOptions<Configuration.AppSettings> settings)
        {
            AppSettings = settings.Value;
        }

        public Queue(AppSettings appSettings)
        {
            // app configs
            AppSettings = appSettings;
        }

        public void QueuePostJson(String jsonCustomerNavigation, String Queue)
        {
            // pass to ConnectionStrings in appsettings.json
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(AppSettings.RabbitMQConnectionString);

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
