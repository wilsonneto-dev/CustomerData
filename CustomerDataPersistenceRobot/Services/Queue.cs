using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Couchbase;
using Couchbase.Authentication;
using CustomerDataPersistenceRobot.Context;
using CustomerDataPersistenceRobot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerDataPersistenceRobot.Services
{
    // queue manage
    class Queue
    {
        // consume data
        public static void QueueStartConsuming()
        {
            // connection setup to RabbitMQ Server
            // pass to connection string
            var factory = new ConnectionFactory()
            {
                HostName = "whale-01.rmq.cloudamqp.com",
                UserName = "yyvswksf",
                VirtualHost = "yyvswksf",
                Password = "aw1hZCbj52fN4U--M3yX9NBSjAMx7xLS"
            };

            // open connection
            using (var connection = factory.CreateConnection())
            {
                // open channel
                using (var channel = connection.CreateModel())
                {
                    // declare
                    channel.QueueDeclare(
                        queue: "Customer",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    // event handler to consume queue
                    var consumer = new EventingBasicConsumer(channel);

                    // callback on receive
                    consumer.Received += (model, ea) =>
                    {
                        // get message on queue
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        // pass json to a object
                        JObject objJson = JObject.Parse(message);
                        CustomerNavigation customerNavigation = new CustomerNavigation()
                        {
                            Date = (DateTime) objJson.GetValue("Date"),
                            Browser = (String) objJson.GetValue("Browser"),
                            PageTitle = (String) objJson.GetValue("PageTitle"),
                            IP = (String) objJson.GetValue("IP"),
                            Params = (String) objJson.GetValue("Params")
                        };
                        Console.WriteLine("* - Received From Queue: {0}", message);

                        // save in SqlServer
                        using (var context = new CustomerDataDbContext())
                        {
                            context.CustomerNavigations.Add(customerNavigation);
                            context.SaveChanges(); // save
                        }
                        Console.WriteLine("- {0}->{1}. Saved in SqlServer. Id: {2}", customerNavigation.IP, customerNavigation.PageTitle, customerNavigation.Id);

                        // save in Couchbase
                        using (var cluster = new Cluster())
                        {
                            // users config
                            // pass to connectionstrings
                            var authenticator = new PasswordAuthenticator("user", "user123");
                            cluster.Authenticate(authenticator);

                            // open bucket
                            using (var bucket = cluster.OpenBucket("CustomerNavigations"))
                            {
                                var document = new Document<dynamic>
                                {
                                    Id = "CustomerNavigation::" + customerNavigation.Id,
                                    Content = customerNavigation
                                };
                                // upser in base
                                var upsert = bucket.Upsert(document);
                                if (upsert.Success)
                                    Console.WriteLine("- {0}->{1} Saved in Couchbase. Id: {2}", customerNavigation.IP, customerNavigation.PageTitle, ("CustomerNavigation::" + customerNavigation.Id.ToString()));
                                else
                                    Console.WriteLine("- * Error {0} Dont saved in Couchbase. Id: {1}", customerNavigation.IP, ("CustomerNavigation::" + customerNavigation.Id.ToString()));
                            }
                        }

                        Console.WriteLine("Go next...");
                        Thread.Sleep(1000);
                    };

                    // setup handler consume queue
                    channel.BasicConsume(
                        queue: "Customer",
                        autoAck: true,
                        consumer: consumer
                    );

                    Console.WriteLine("Queue consuming start...");
                    Console.ReadLine();
                }
            }
        }

    }
}
