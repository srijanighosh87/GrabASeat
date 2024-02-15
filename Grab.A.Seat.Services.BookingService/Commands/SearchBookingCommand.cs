

using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class SearchBookingCommand : ICommand
    {
        public string nameOrRef { get; set; }
        public DateTime date { get; set; }
    }
}
