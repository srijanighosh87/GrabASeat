
using Grab.A.seat.BookingService;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.Manager;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Grab.A.Seat.BookingAPI.Bookings.Managers
{
    public class UpdateBookingManager : BaseManager<UpdateBookingCommand>
    {
        private readonly ILogger<UpdateBookingManager> _logger;
        ResponseDto _responseDto;
        private readonly ApplicationDbContext _dbContext;


        public UpdateBookingManager(
            ILogger<UpdateBookingManager> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            _dbContext = dbContext;
        }
        public async Task<ResponseDto> ProcessAsync(UpdateBookingCommand command)
        {
            try
            {
                var mappedObj = new Booking
                {
                    PartySize = command.PartySize,
                    BookingStartDateTime = command.BookingDateTime,
                    Comments = command.Comments,
                };

                //check date
                if (command.BookingDateTime > new DateTime() && command.BookingDateTime < DateTime.Now) 
                    return WrapResponse.WrapError(_responseDto, "Booking Date can't be in the past", logger: _logger);

                //get booking from DB
                var existingBooking = await _dbContext.Bookings
                    .Include(b => b.Table)
                    .Include(b => b.Customer)
                    .FirstOrDefaultAsync(b => b.BookingReference == command.BookingReference);
                if (existingBooking == null) return WrapResponse.WrapError(_responseDto, "Booking not found!", logger: _logger) ;
                mappedObj.Id = existingBooking.Id;

                //check capacity and reschedule table if needed
                await ModifyBooking(command, existingBooking, mappedObj);

                

                return WrapResponse
                    .WrapOk(_responseDto, 
                    $"Returninging Booking with reference {existingBooking.BookingReference}", BookingHelpers.ConvertBookingToDto(existingBooking));
            }
            catch (Exception e)
            {
                return WrapResponse.WrapError(_responseDto, e.Message.ToString(), logger: _logger);
            }

        }
        private async Task ModifyBooking(UpdateBookingCommand command, Booking existingBooking, Booking mappedObj)
        {

            try
            {
                //if booking time didnt change
                if (command.BookingDateTime == null || command.BookingDateTime == new DateTime())
                {
                    // if party size same or less and datetime is same, no need to change the table
                    if (command.PartySize <= existingBooking.PartySize)
                    {
                        //await _bookingRepository.UpdateAsync(mappedObj);
                        await UpdateBooking(mappedObj);
                        return;
                    }

                    // if partysize more than previous

                    //1. Check if new party size still can be handled by current booked table
                    // just update other details
                    if (command.PartySize <= existingBooking.Table.Capacity)
                    {
                        //await _bookingRepository.UpdateAsync(mappedObj);
                        await UpdateBooking(mappedObj);
                        return;
                    }

                    //2. Check if new party size can't be handled by current booked table
                    // search for another table
                    await SearchTable(command, existingBooking.BookingStartDateTime, existingBooking.BookingEndDateTime, existingBooking);
                }
                else // booking time changed
                {
                    var BookingStartTime = command.BookingDateTime;
                    var BookingEndTime = BookingStartTime.AddHours(2);

                    //do a new search
                    await SearchTable(command, BookingStartTime, BookingEndTime, existingBooking);
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message.ToString());
            }
        }

        
        private async Task SearchTable(UpdateBookingCommand command, 
            DateTime bookingStartTime, 
            DateTime bookingEndTime, 
            Booking existingBooking)
        {
            var freeTable = await BookingHelpers.CheckAvailability(command.BookingDateTime, command.BookingDateTime.AddHours(2), command.PartySize,_dbContext);
            existingBooking.Table = freeTable;
            existingBooking.PartySize = command.PartySize;
            existingBooking.BookingStartDateTime = bookingStartTime;
            existingBooking.BookingEndDateTime = bookingEndTime;
            existingBooking.Comments= command.Comments;

            await UpdateBooking(existingBooking);
        }

        private async Task UpdateBooking(Booking booking)
        {
            var existingBooking = _dbContext.Bookings.Include(b => b.Customer).FirstOrDefault(b => b.Id == booking.Id);
            if (existingBooking == null) throw new Exception("Booking to update could not be found!");
            if (existingBooking != null && booking != null)
            {
                var properties = typeof(Booking).GetProperties();
                foreach (var property in properties)
                {
                    //do not set values for FKs
                    if (property.Name == "Id" ||
                        property.Name == "CreationDate" ||
                        property.Name.ToLower().Contains("_id") ||
                        property.Name == "BookingEndDateTime"
                        )
                        continue;

                    // ignore setting date to default 01/01/0001 00:00:00 or 01/01/0001 02:00:00
                    if (property.PropertyType == typeof(DateTime) &&
                        ((DateTime)property.GetValue(booking) == new DateTime()
                        || (DateTime)property.GetValue(booking) == new DateTime().AddHours(2)))
                        continue;

                    var updatedValue = property.GetValue(booking);
                    if (updatedValue != null && updatedValue != property.GetValue(existingBooking))
                    {
                        property.SetValue(existingBooking, updatedValue);
                    }
                }
            }
            await _dbContext.SaveChangesAsync();

        }

    }
}
