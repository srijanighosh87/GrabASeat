
namespace Grab.A.Seat.BookingAPI.Bookings.Dto
{
    public class BookingDto
    {
        public Guid? Id { get; set; }
        public string? bookingReference { get; set; }
        public string? CustomerName { get; set; }
        public string? TableNumber { get; set; }
        public int? PartySize { get; set; }
        public DateTime? BookingStartDateTime { get; set; }
        public string? Comments { get; set; }
    }
}
