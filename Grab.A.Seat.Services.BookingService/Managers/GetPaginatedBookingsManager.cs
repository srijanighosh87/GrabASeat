

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
    public class GetPaginatedBookingsManager : BaseGetAllManager<GetPaginatedBookingCommand>
    {
        private readonly ILogger<GetPaginatedBookingsManager> _logger;
        ResponseDto _responseDto;
        private readonly ApplicationDbContext _dbContext;


        public GetPaginatedBookingsManager(
            ILogger<GetPaginatedBookingsManager> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            this._dbContext = dbContext;
        }
        public async Task<ResponseDto> ProcessAsync(GetPaginatedBookingCommand command)
        {
            try
            {
                IQueryable allBookings;
                int count = 0;
                if (command.fetchPastBookings)
                {
                    allBookings = _dbContext.Bookings
                        .Include(c => c.Customer)
                        .Include(t => t.Table)
                        .OrderByDescending(a => a.BookingStartDateTime)
                        .Skip((command.pageNumber - 1) * command.numberOfItemsPerPage)
                        .Take(command.numberOfItemsPerPage);
                    count = _dbContext.Bookings.Count();
                }
                else
                {
                    allBookings = _dbContext.Bookings
                        .Include(c => c.Customer)
                        .Include(t => t.Table)
                        .OrderByDescending(a => a.BookingStartDateTime)
                        .Skip((command.pageNumber - 1) * command.numberOfItemsPerPage)
                        .Take(command.numberOfItemsPerPage);
                    count = _dbContext.Bookings.Count();
                }
                
                var allBookingsAsOBjects = allBookings.OfType<Booking>();
                var allDtos = new List<BookingDto>();

                foreach (var bookingObj in allBookingsAsOBjects)
                {
                    allDtos.Add(BookingHelpers.ConvertBookingToDto(bookingObj));
                }

                return WrapResponse.WrapOk(_responseDto, "Returning All Bookings", new
                {
                    allDtos = allDtos,
                    count = count
                });
            }
            catch (Exception e)
            {
                return WrapResponse.WrapError(_responseDto, e.Message.ToString(), logger: _logger);
            }
        }
    }
}
