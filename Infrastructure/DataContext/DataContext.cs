using UserOrdersApiDemo.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace UserOrdersApiDemo.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Order> Order { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasData(new Role { Id = 1, Name = "User" });

            builder.Entity<Role>()
                .HasData(new Role { Id = 2, Name = "Admin" });
        }
    }
}