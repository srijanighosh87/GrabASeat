

using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Grab.A.Seat.Domains.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }

        #region Tables
        public DbSet<Booking> Bookings { get; set; } = null;
        public DbSet<Table> Tables { get; set; } = null;
        public DbSet<Customer> Customers { get; set; } = null;
        public DbSet<Sequence> Sequences { get; set; } = null;


        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Customer>()
                   .HasIndex(c => new { c.Name, c.ContactNumber })
                   .IsUnique();

            modelBuilder.Entity<Customer>().HasData(
                 new Customer
                 {
                     Id = Guid.Parse("3dd83670-a736-48fc-838f-bceab7a30735"),
                     Name = "Srijani",
                     Email = "srijanighosh87@gmail.com",
                     ContactNumber = "9876541232",
                     CreationDate = DateTime.UtcNow
                 },
                 new Customer
                 {
                     Id = Guid.Parse("54f3537a-7724-4fad-bb5e-348cc9a4d19d"),
                     Name = "Sricheta",
                     Email = "sricheta1994@gmail.com",
                     ContactNumber = "1234567895",
                     CreationDate = DateTime.UtcNow
                 }
            );

            modelBuilder.Entity<Table>().HasData(
                new Table
                {
                    Id = Guid.Parse("220105b1-487d-4e9d-b2df-21ce54d30051"),
                    TableNumber = "Table01",
                    Capacity = 2
                },
                new Table
                {
                    Id = Guid.Parse("afd7fea7-3bfb-4231-99db-02bbd19304fb"),
                    TableNumber = "Table02",
                    Capacity = 5
                },
                new Table
                {
                    Id = Guid.Parse("4e221cd3-1111-44e8-849f-1ab4770dd17e"),
                    TableNumber = "Table03",
                    Capacity = 2
                },
                new Table
                {
                    Id = Guid.Parse("74981a7f-8441-48ab-a10c-5e58f368f4f6"),
                    TableNumber = "Table04",
                    Capacity = 3
                }
            );



            modelBuilder.Entity<Sequence>().HasData(
                 new Sequence
                 {
                     Start = 1,
                     IncreaseBy = 1,
                     CuurentValue = 1,
                     CreationDate = DateTime.UtcNow,
                     SequenceName = "Booking_Reference"
                 }
            );

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingStartDateTime = DateTime.UtcNow,
                    BookingEndDateTime = DateTime.UtcNow.AddHours(2),
                    Comments = "Non Veg Only",
                    CreationDate = DateTime.UtcNow,
                    PartySize = 2,
                    Customer_Id = Guid.Parse("3dd83670-a736-48fc-838f-bceab7a30735"),
                    Table_Id = Guid.Parse("220105b1-487d-4e9d-b2df-21ce54d30051"),
                    BookingReference = "0001"
                },
                new Booking
                {
                    BookingStartDateTime = DateTime.UtcNow,
                    BookingEndDateTime = DateTime.UtcNow.AddHours(2),
                    Comments = "Window seat",
                    CreationDate = DateTime.UtcNow,
                    PartySize = 2,
                    Customer_Id = Guid.Parse("54f3537a-7724-4fad-bb5e-348cc9a4d19d"),
                    Table_Id = Guid.Parse("afd7fea7-3bfb-4231-99db-02bbd19304fb"),
                    BookingReference = "0002"
                },
                new Booking
                {
                    BookingStartDateTime = DateTime.UtcNow,
                    BookingEndDateTime = DateTime.UtcNow.AddHours(2),
                    Comments = "",
                    CreationDate = DateTime.UtcNow,
                    PartySize = 2,
                    Customer_Id = Guid.Parse("54f3537a-7724-4fad-bb5e-348cc9a4d19d"),
                    Table_Id = Guid.Parse("4e221cd3-1111-44e8-849f-1ab4770dd17e"),
                    BookingReference = "0003"
                }
            );


        }


    }
}
