using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class UpdateBookingCommand : ICommand
    {
        public string BookingReference { get; set; }
        public int PartySize { get; set; }
        public DateTime BookingDateTime { get; set; }
        public string? Comments { get; set; }
        

    }
}
