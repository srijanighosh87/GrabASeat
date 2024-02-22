using Grab.A.Seat.Shared.Commands;
using System.ComponentModel.DataAnnotations;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class SendEmailCommand : ICommand
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string RefNo { get; set; }
        public DateTime Date { get; set; }

    }
}
