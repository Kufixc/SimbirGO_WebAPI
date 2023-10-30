using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimbirGO_API.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int DurationDays { get; set; }

        public int DurationHours { get; set; }
        [Required]
        public int DurationMinutes { get; set; }

        public double TotalCost { get; set; }

        [ForeignKey("Transport")]
        public int TransportId { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
    }



}
