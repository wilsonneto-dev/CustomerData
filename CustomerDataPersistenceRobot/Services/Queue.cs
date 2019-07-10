using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Couchbase;
using Couchbase.Authentication;
using CustomerDataPersistenceRobot.Configuration;
using CustomerDataPersistenceRobot.Context;
using CustomerDataPersistenceRobot.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CustomerDataPersistenceRobot.Services
{
    // queue manage
    class Queue
    {
        private readonly AppSettings AppSettings;
        public Queue(IOptions<Configuration.AppSettings> settings)
        {
            AppSettings = settings.Value;
        }

        public Queue(AppSettings appSettings)
        {
            // app configs
            AppSettings = appSettings;
        }

        // consume data
        public void QueueStartConsuming()
        {
            // pass to ConnectionStrings in appsettings.json
            var factory = new ConnectionFactory();
            factory.Uri = new Uri(this.AppSettings.RabbitMQConnectionString);

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
                        try
                        {
                            using (var context = new CustomerDataDbContext())
                            {
                                context.CustomerNavigations.Add(customerNavigation);
                                context.SaveChanges(); // save
                            }
                            Console.WriteLine("- {0}->{1}. Saved in SqlServer. Id: {2}", customerNavigation.IP, customerNavigation.PageTitle, customerNavigation.Id);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("- {0}->{1}. Error * SQL: {2}", customerNavigation.IP, customerNavigation.PageTitle, ex.Message);
                        }

                        try
                        {
                            // save in Couchbase
                            using (var cluster = new Cluster())
                            {
                                // users config
                                // pass to connectionstrings
                                var authenticator = new PasswordAuthenticator(AppSettings.NoSqlUser, AppSettings.NoSqlPass);
                                cluster.Authenticate(authenticator);

                                // open bucket
                                using (var bucket = cluster.OpenBucket(AppSettings.NoSqlBucket))
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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("- {0}->{1}. Error * NoSQL: {2}", customerNavigation.IP, customerNavigation.PageTitle, ex.Message);
                        }

                        // just log and wait 1 second
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
