using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Dto;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Grab.A.seat.BookingService
{
    public static class BookingHelpers
    {
        public static async Task<Customer> AddCustomerToDb<T>(AddBookingCommand command, ApplicationDbContext dbContext, ILogger<T> logger)
        {
            var addedCustomerEntity = await dbContext.Customers.AddAsync(new Customer
            {
                Name = command.CustomerName,
                ContactNumber = command.CustomerContact,
            });

            dbContext.SaveChanges();
            logger.LogInformation($"Customer created with Id : {addedCustomerEntity.Entity.Id}");

            return addedCustomerEntity.Entity;
        }

        public static async Task<Table> CheckAvailability(DateTime BookingDateTime, DateTime BookingEndTime, int PartySize, ApplicationDbContext dbContext)
        {
            var freeTable = await dbContext.Tables.Include(table => table.Bookings).Where(table =>
                        table.Capacity >= PartySize
                        &&
                        !table.Bookings.Any(booking =>
                           booking.BookingStartDateTime < BookingEndTime &&
                            booking.BookingEndDateTime > BookingDateTime
                        )
                    )
                    .FirstOrDefaultAsync();

            return freeTable;
        }

        public static BookingDto ConvertBookingToDto(Booking booking)
        {
            return new BookingDto
            {
                bookingReference = booking.BookingReference,
                BookingStartDateTime = booking.BookingStartDateTime,
                Comments = booking.Comments,
                CustomerName = booking.Customer.Name,
                Id = booking.Id,
                PartySize = booking.PartySize,
                TableNumber = booking.Table.TableNumber
            };
        }
    }
}
