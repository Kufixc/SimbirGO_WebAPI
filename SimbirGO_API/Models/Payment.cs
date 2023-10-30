using System.ComponentModel.DataAnnotations;

namespace SimbirGO_API.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public DateTime DateOperazion { get; set; }

        public int ClientID { get; set; }
    }
}
