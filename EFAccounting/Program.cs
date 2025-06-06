using EFAccounting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFAccounting
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Load connection string from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultDb");

            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            using var ctx = new Context(optionsBuilder.Options);

            if (ctx.Database.EnsureCreated())
            {
                Console.WriteLine("The database has been created.");
                CreateDemoData();
            }
            else
            {
                Console.WriteLine("Database existed already.");

                if (!ctx.Kids.Any())
                    CreateDemoData();
            }

            void CreateDemoData()
            {
                Console.WriteLine("Creating demo data, please wait...");

                Kid Leticia = new()
                {
                    BirthDate = new DateOnly(2015, 3, 14),
                    Name = "Leticia",
                    LastName = "Magallhaes",
                    SiblingFrom = new List<Kid>(),
                    SiblingTo = new List<Kid>(),
                };
                Kid Carolina = new()
                {
                    BirthDate = new DateOnly(2018, 12, 5),
                    Name = "Carolina",
                    LastName = "Magallhaes",
                    SiblingTo = new List<Kid>(),
                    SiblingFrom = new List<Kid>()
                };

                Leticia.SiblingTo.Add(Carolina);
                Carolina.SiblingFrom.Add(Leticia);
                Carolina.SiblingTo.Add(Leticia);
                Leticia.SiblingFrom.Add(Carolina);

                ctx.Kids.AddRange(Leticia, Carolina);
                ctx.SaveChanges();

                var price = new Price { Label = "Default price", Value = 5.0f };
                ctx.Prices.Add(price);
                ctx.SaveChanges();

                List<WDay> wds =
                [
                    new() { Kid = Leticia, Arrival = new TimeOnly(6, 30), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(8, 0), Price = price },
                    new() { Kid = Leticia, Arrival = new TimeOnly(11, 45), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(13, 0), Price = price },
                    new() { Kid = Leticia, Arrival = new TimeOnly(16, 15), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(16, 30), Price = price },
                    new() { Kid = Carolina, Arrival = new TimeOnly(6, 30), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(8, 0), Price = price },
                    new() { Kid = Carolina, Arrival = new TimeOnly(11, 45), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(13, 0), Price = price },
                    new() { Kid = Carolina, Arrival = new TimeOnly(16, 15), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(16, 30), Price = price },
                ];

                ctx.Wdays.AddRange(wds);
                ctx.SaveChanges();

                Console.WriteLine("Demo data written successfully!");
            }
        }
    }
}
