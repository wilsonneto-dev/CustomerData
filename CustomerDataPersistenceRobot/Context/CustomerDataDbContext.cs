using CustomerDataPersistenceRobot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerDataPersistenceRobot.Context
{
    class CustomerDataDbContext : DbContext
    {
        // public CustomerDataDbContext(DbContextOptions<CustomerDataDbContext> options) : base(options)
        // {}

        public DbSet<CustomerNavigation> CustomerNavigations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=ViajaNet;Trusted_Connection=True;");
        }

    }
}
