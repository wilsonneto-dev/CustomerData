using CustomerDataPersistenceRobot.Context;
using CustomerDataPersistenceRobot.Models;
using CustomerDataPersistenceRobot.Utility;
using System;

namespace CustomerDataPersistenceRobot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            // QueueRabbitMQ.QueueConsume();

            using (var context = new CustomerDataDbContext())
            {

                CustomerNavigation customerNavigation = new CustomerNavigation()
                {
                    IP = "x",
                    Date = DateTime.Now,
                    PageTitle = "Teste",
                    Params = "Params"
                };

                context.CustomerNavigations.Add(customerNavigation);
                context.SaveChanges();
            }

            Console.WriteLine("Ok... Press [enter] to exit");
            Console.ReadLine();

        }
    }
}
