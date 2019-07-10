using CustomerDataPersistenceRobot.Context;
using CustomerDataPersistenceRobot.Models;
using System;
using Couchbase;
using Couchbase.Authentication;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using CustomerDataPersistenceRobot.Configuration;

namespace CustomerDataPersistenceRobot
{
    class Program
    {
        public static AppSettings Settings;

        static void Main(string[] args)
        {
            // log
            Console.WriteLine("Initializing...");

            // config load
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Settings = new AppSettings();
            builder.Build().Bind("AppSettings", Settings);

            // wait for user
            Console.WriteLine("Press [enter] to start.");
            Console.WriteLine("Ok...");

            // initialize queue  consuming
            Services.Queue queue = new Services.Queue(Settings);
            queue.QueueStartConsuming();

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

        }
    }
}
