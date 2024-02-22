using Grab.A.Seat.Shared.Commands;
using System.ComponentModel.DataAnnotations;

namespace Grab.A.Seat.BookingAPI.Bookings.Commands
{
    public class AddBookingCommand : ICommand
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerContact { get; set; }
        [Range(1,10, ErrorMessage = "Can be only between 1 and 10")]
        public int PartySize { get; set; }
        [Required]
        public DateTime BookingDateTime { get; set; }
        public string? Comments { get; set; }
        public string? Email { get; set; }


    }
}
