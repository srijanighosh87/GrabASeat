

using Grab.A.seat.BookingService;
using Grab.A.Seat.BookingAPI.Bookings.Commands;
using Grab.A.Seat.BookingAPI.Bookings.Dto;
using Grab.A.Seat.Domains.Data;
using Grab.A.Seat.Shared.Dtos;
using Grab.A.Seat.Shared.ManagerConfig;
using Grab.A.Seat.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Grab.A.Seat.BookingAPI.Bookings.Managers
{
    public class SearchBookingsManager : BaseGetAllManager<SearchBookingCommand>
    {
        private readonly ILogger<SearchBookingsManager> _logger;
        ResponseDto _responseDto;
        private readonly ApplicationDbContext _dbContext;


        public SearchBookingsManager(
            ILogger<SearchBookingsManager> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _responseDto = new ResponseDto();
            this._dbContext = dbContext;
        }
        public async Task<ResponseDto> ProcessAsync(SearchBookingCommand command)
        {
            try
            {
                IQueryable allBookings = Enumerable.Empty<Booking>().AsQueryable();
                if (command.nameOrRef != null && command.date == new DateTime())
                    allBookings = _dbContext.Bookings
                    .Include(c => c.Customer)
                    .Include(t => t.Table)
                    .Where(b => b.BookingReference == command.nameOrRef || b.Customer.Name.ToLower().Contains(command.nameOrRef.ToLower()))
                    .OrderByDescending(a => a.BookingStartDateTime);

                if (command.nameOrRef.IsNullOrEmpty() && command.date != new DateTime())
                    allBookings = _dbContext.Bookings
                    .Include(c => c.Customer)
                    .Include(t => t.Table)
                    .Where(b => b.BookingStartDateTime.Date.CompareTo(command.date.Date) == 0)
                    .OrderByDescending(a => a.BookingStartDateTime);

                if (!command.nameOrRef.IsNullOrEmpty() && command.date != new DateTime())
                    allBookings = _dbContext.Bookings
                    .Include(c => c.Customer)
                    .Include(t => t.Table)
                    .Where(b => b.BookingStartDateTime.Date.CompareTo(command.date.Date) == 0)
                    .Where(b => b.BookingReference == command.nameOrRef || b.Customer.Name.ToLower().Contains(command.nameOrRef.ToLower()))
                    .OrderByDescending(a => a.BookingStartDateTime);

                var allBookingsAsOBjects = allBookings.OfType<Booking>();
                var allDtos = new List<BookingDto>();

                foreach (var bookingObj in allBookingsAsOBjects)
                {
                    allDtos.Add(BookingHelpers.ConvertBookingToDto(bookingObj));
                }

                return WrapResponse.WrapOk(_responseDto, "Returning All Bookings", new
                {
                    allDtos = allDtos,
                    count = allBookingsAsOBjects.Count()
                });
            }
            catch (Exception e)
            {
                return WrapResponse.WrapError(_responseDto, e.Message.ToString(), logger: _logger);
            }
        }
    }
}
