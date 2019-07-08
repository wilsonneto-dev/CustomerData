using CustomerDataPersistenceRobot.Context;
using CustomerDataPersistenceRobot.Models;
using System;
using Couchbase;
using Couchbase.Authentication;
using System.Collections.Generic;

namespace CustomerDataPersistenceRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            // log
            Console.WriteLine("Initializing...");

            // initialize queue  consuming
            Services.Queue.QueueStartConsuming();
            
            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

        }
    }
}
