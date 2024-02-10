
using Grab.A.Seat.Shared.Domains.Models;
using System.ComponentModel.DataAnnotations;


namespace Grab.A.Seat.Shared.Models
{
    public class Table : BaseModel
    {
        [Required]
        public string TableNumber { get; set; }
        [Required]
        public int Capacity { get; set; }
        public List<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
