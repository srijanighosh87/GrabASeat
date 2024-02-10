using System.ComponentModel.DataAnnotations;

namespace Grab.A.Seat.Shared.Domains.Models
{
    public class BaseModel
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
