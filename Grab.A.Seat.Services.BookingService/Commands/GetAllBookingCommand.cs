﻿

using Grab.A.Seat.Shared.Commands;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class GetAllBookingCommand : ICommand
    {
        public bool fetchPastBookings { get; set; }
    }
}
