using EFAccounting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFAccounting
{
    public class Context : DbContext
    {
        public DbSet<Kid> Kids { get; set; }
        public DbSet<WDay> Wdays { get; set; }
        public DbSet<Price> Prices { get; set; }

        public string DbPath { get; }

        // Deliberate empty constructor
        public Context() { }

        // Create relations between kids to store siblings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kid>()
                .HasMany(k => k.SiblingFrom)
                .WithMany(k => k.SiblingTo)
                .UsingEntity(e => e.ToTable("KidSiblings"));
        }

        // Load connection string from appsettings.json file
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.UseLazyLoadingProxies();
            builder.UseMySql(config.GetConnectionString("DefaultDb"), new MySqlServerVersion(new Version()));
        }
    }
}
