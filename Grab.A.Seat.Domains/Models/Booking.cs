
using Grab.A.Seat.Shared.Domains.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Grab.A.Seat.Shared.Models
{
    public class Booking : BaseModel
    {
        [ForeignKey("Customer")]
        public Guid Customer_Id { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }
        [ForeignKey("Table")]
        public Guid Table_Id { get; set; }
        [JsonIgnore]
        public Table Table { get; set; }

        [Required]
        [Range(1,10, ErrorMessage="Party Size can only be between 1 and 10")]
        public int PartySize { get; set; }
        [Required]
        public DateTime BookingStartDateTime { get; set; }
        public DateTime BookingEndDateTime { get; set; }

        public string? Comments { get; set; }
        [Required]
        public string BookingReference { get; set; }
    }
}
