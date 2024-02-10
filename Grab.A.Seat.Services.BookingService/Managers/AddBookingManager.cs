

using FluentValidation;
using Grab.A.seat.BookingService;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Dto;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.Manager;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Grab.A.Seat.BookingAPI.Bookings.Managers
{
    public class AddBookingManager : BaseManager<AddBookingCommand>
    {
        private readonly ILogger<AddBookingManager> _logger;
        private readonly ResponseDto _responseDto;
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Booking> _validator;



        public AddBookingManager(
            ILogger<AddBookingManager> logger,
            ApplicationDbContext dbContext,
            IValidator<Booking> validator
            )
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            _dbContext = dbContext;
            _validator = validator;
        }
        public async Task<ResponseDto> ProcessAsync(AddBookingCommand command)
        {
            try
            {
                var bookingToAdd = new Booking
                {
                    PartySize = command.PartySize,
                    BookingStartDateTime = command.BookingDateTime,
                    Comments = command.Comments,
                    BookingEndDateTime= command.BookingDateTime.AddHours(2),
                };

                //Check if customer exists
                var customer = _dbContext.Customers
                    .FirstOrDefault(s => s.ContactNumber == command.CustomerContact && s.Name == command.CustomerName);
                if (customer == null)
                {
                    //create Customer in DB and add it to booking
                    //customer = await AddCustomerToDb(command);
                    customer = await BookingHelpers.AddCustomerToDb(command, _dbContext, _logger);
                    bookingToAdd.Customer = customer;
                }
                else
                {
                    //add existing customer to booking
                    bookingToAdd.Customer = customer;
                }

                //Check availability for tables and add
                Table freeTable = await BookingHelpers.CheckAvailability(command.BookingDateTime, command.BookingDateTime.AddHours(2), command.PartySize,_dbContext);
                if (freeTable == null) return WrapResponse.WrapError(_responseDto, "No Free Tables found for the Selected time and party size!", logger: _logger);
                bookingToAdd.Table = freeTable;

                //add booking reference
                var seq = await _dbContext.Sequences.FirstOrDefaultAsync(a => a.SequenceName == "Booking_Reference");
                if(seq == null) WrapResponse.WrapError(_responseDto, "Seq for Booking Refrence could not be found!", logger: _logger);
                bookingToAdd.BookingReference = seq.CuurentValue.ToString();
                seq.CuurentValue++;

                //call validator and add to DB
                var res =  await _validator.ValidateAsync(bookingToAdd, CancellationToken.None);
                if (res.IsValid)
                {
                    await _dbContext.Bookings.AddAsync(bookingToAdd);
                    _dbContext.SaveChanges();
                }

                else
                {
                    var errorMessage = string.Join(", ", res.Errors.Select(a => a.ErrorMessage));
                    return WrapResponse.WrapError(_responseDto, errorMessage, logger: _logger);
                }
                

                return WrapResponse.WrapOk(_responseDto, "Booking Created", BookingHelpers.ConvertBookingToDto(bookingToAdd));
            }
            catch (Exception e)
            {
                return WrapResponse.WrapError(_responseDto, e.Message.ToString(), logger: _logger);
            }

        }

    }
}
