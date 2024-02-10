
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.Manager;
using Microsoft.EntityFrameworkCore;

namespace Grab.A.Seat.BookingAPI.Bookings.Managers
{
    public class DeleteBookingManager : BaseManager<DeleteBookingCommand>
    {
        private readonly ILogger<DeleteBookingManager> _logger;
        ResponseDto _responseDto;
        private readonly ApplicationDbContext _dbContext;

        public DeleteBookingManager(
            ILogger<DeleteBookingManager> logger,
             ApplicationDbContext dbContext
            )
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            _dbContext = dbContext;
        }
        public async Task<ResponseDto> ProcessAsync(DeleteBookingCommand command)
        {
            try
            {
                var deleteObj = await _dbContext.Bookings.FindAsync(command.Id);
                if (deleteObj == null) throw new Exception($"{command.Id} not found!");
                _dbContext.Bookings.Remove(deleteObj);
                _dbContext.SaveChanges();   
                return WrapResponse.WrapOk(_responseDto, "Booking Deleted", command.Id);
            }
            catch (Exception e)
            {
                return WrapResponse.WrapError(_responseDto, e.Message.ToString(), logger: _logger);
            }

        }
    }
}
