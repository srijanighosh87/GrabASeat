
using Grab.A.seat.BookingService;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Dto;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.Manager;
using Microsoft.EntityFrameworkCore;

namespace Grab.A.Seat.BookingAPI.Bookings.Managers
{
    public class GetBookingByIdManager : BaseManager<GetBookingByIdCommand>
    {
        private readonly ILogger<GetBookingByIdManager> _logger;
        ResponseDto _responseDto;
        private readonly ApplicationDbContext _dbContext;


        public GetBookingByIdManager(
            ILogger<GetBookingByIdManager> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            _dbContext = dbContext;
        }
        public async Task<ResponseDto> ProcessAsync(GetBookingByIdCommand command)
        {
            try
            {
                var bookingObj = await _dbContext.Bookings.Include(c => c.Customer).Include(t => t.Table).FirstOrDefaultAsync(b => b.Id == command.id);
                if (bookingObj == null) return WrapResponse.WrapError(_responseDto, $"Booking {command.id} not found!", logger: _logger);
                return WrapResponse.WrapOk(_responseDto, "Booking Found!", BookingHelpers.ConvertBookingToDto(bookingObj));
            }
            catch (Exception e)
            {
                return WrapResponse.WrapError(_responseDto, e.Message.ToString(), logger: _logger);
            }

        }
    }
}
