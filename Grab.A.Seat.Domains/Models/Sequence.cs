
using Grab.A.Seat.Shared.Domains.Models;
using System.ComponentModel.DataAnnotations;


namespace Grab.A.Seat.Shared.Models
{
    public class Sequence : BaseModel
    {
        [Required]
        public double Start { get; set; }
        [Required]
        public double IncreaseBy { get; set; }
        [Required]
        public string SequenceName { get; set; }
        [Required]
        public double CuurentValue { get; set; }

    }
}
