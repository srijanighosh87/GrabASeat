
using Grab.A.Seat.Shared.Domains.Models;
using Grab.A.Seat.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Grab.A.Seat.Shared.Models
{
    public class Customer : BaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        public string? Email { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
