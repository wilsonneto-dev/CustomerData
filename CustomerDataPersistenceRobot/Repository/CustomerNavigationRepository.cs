using Couchbase;
using Couchbase.Authentication;
using CustomerDataPersistenceRobot.Configuration;
using CustomerDataPersistenceRobot.Context;
using CustomerDataPersistenceRobot.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerDataPersistenceRobot.Repository
{
    class CustomerNavigationRepository
    {
        private readonly AppSettings AppSettings;
        public CustomerNavigationRepository(IOptions<Configuration.AppSettings> settings)
        {
            AppSettings = settings.Value;
        }
        public CustomerNavigationRepository(AppSettings settings)
        {
            AppSettings = settings;
        }

        public void AddSql(CustomerNavigation customerNavigation)
        {
            using (var context = new CustomerDataDbContext())
            {
                context.CustomerNavigations.Add(customerNavigation);
                context.SaveChanges();
            }
        }
            
        // NoSQL
        public bool AddNoSql(CustomerNavigation customerNavigation)
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
                    // upset in base
                    var upsert = bucket.Upsert(document);
                    return (upsert.Success);

                }
            }

        }

    }
}
