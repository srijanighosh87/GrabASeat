using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class DeleteBookingCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
