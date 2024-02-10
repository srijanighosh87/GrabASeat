

using Grab.A.seat.BookingService;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Dto;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.ManagerConfig;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Grab.A.Seat.BookingAPI.Bookings.Managers
{
    public class GetAllBookingsManager : BaseGetAllManager<GetAllBookingCommand>
    {
        private readonly ILogger<GetAllBookingsManager> _logger;
        ResponseDto _responseDto;
        private readonly ApplicationDbContext _dbContext;


        public GetAllBookingsManager(
            ILogger<GetAllBookingsManager> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            this._dbContext = dbContext;
        }
        public async Task<ResponseDto> ProcessAsync(GetAllBookingCommand command)
        {
            try
            {

                IQueryable allBookings;
                if(command.isTrackingEnabled)
                    allBookings = _dbContext.Bookings.Include(c => c.Customer).Include(t => t.Table).OrderByDescending(a => a.BookingStartDateTime);
                else
                    allBookings = _dbContext.Bookings.Include(c => c.Customer).Include(t => t.Table).OrderByDescending(a => a.BookingStartDateTime).AsNoTracking();

                var x = allBookings.OfType<Booking>();
                var allDtos = new List<BookingDto>();

                foreach (var bookingObj in x)
                {
                    allDtos.Add(BookingHelpers.ConvertBookingToDto(bookingObj));
                }

                return WrapResponse.WrapOk(_responseDto, "Returning All Bookings", x);
            }
            catch (Exception e)
            {
                return WrapResponse.WrapError(_responseDto, e.Message.ToString(), logger: _logger);
            }
        }
    }
}
