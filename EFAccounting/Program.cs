using EFAccounting.Entities;

namespace EFAccounting
{
    public class Program
    {
        static Context ctx;
        static void Main(string[] args)
        {
            ctx = new Context();

            if (ctx.Database.EnsureCreated())
            {
                // Database didn't exist
                Console.WriteLine("The database has been created.");
                CreateDemoData();
            }
            else
            {
                // Database existed already
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

                ctx.Kids.Add(Leticia);
                ctx.Kids.Add(Carolina);

                ctx.SaveChanges();


                Price price = new() { Label = "Default price", Value = 5.0f };
                ctx.Prices.Add(price);
                ctx.SaveChanges();


                WDay wd1 = new WDay { Kid = Leticia, Arrival = new TimeOnly(6, 30), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(8, 0), Price =  price};
                WDay wd2 = new WDay { Kid = Leticia, Arrival = new TimeOnly(11, 45), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(13, 0), Price = price };
                WDay wd3 = new WDay { Kid = Leticia, Arrival = new TimeOnly(16, 15), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(16, 30), Price = price };


                WDay wd11 = new WDay { Kid = Carolina, Arrival = new TimeOnly(6, 30), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(8, 0), Price = price };
                WDay wd12 = new WDay { Kid = Carolina, Arrival = new TimeOnly(11, 45), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(13, 0), Price = price };
                WDay wd13 = new WDay { Kid = Carolina, Arrival = new TimeOnly(16, 15), Date = new DateOnly(2025, 3, 25), Departure = new TimeOnly(16, 30), Price = price };

                List<WDay> wds = [wd1, wd2, wd3, wd11, wd12, wd13];

                ctx.Wdays.AddRange(wds);

                ctx.SaveChanges();


                Console.WriteLine("Demo data written successfully!");
            }
        }
    }
}
