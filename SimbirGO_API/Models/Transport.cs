using System.ComponentModel.DataAnnotations;

namespace SimbirGO_API.Models
{
    public class Transport
    { 
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        public double Speed { get; set; }

        [Required]
        public double RentPrice { get; set; }

        public string Color { get; set; }

        [Required]
        public bool Availability { get; set; }

    }
}
