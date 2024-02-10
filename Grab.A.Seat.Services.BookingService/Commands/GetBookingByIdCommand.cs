

using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class GetBookingByIdCommand : ICommand
    {
        public Guid id { get; set; } 
    }
}
