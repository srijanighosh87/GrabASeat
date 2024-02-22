using Grab.A.Seat.Shared.Commands;
using System.ComponentModel.DataAnnotations;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class SendEmailCommand : ICommand
    {
        public string Email;
        public string Name;
        public string RefNo { get; set; }
    }
}
