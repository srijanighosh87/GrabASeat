

using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class GetPaginatedBookingCommand : ICommand
    {
        public bool fetchPastBookings { get; set; }
        public int pageNumber { get; set; }
        public int numberOfItemsPerPage { get; set; }
    }
}
