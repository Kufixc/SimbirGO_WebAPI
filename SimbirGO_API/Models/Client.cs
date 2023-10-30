using System.ComponentModel.DataAnnotations;

namespace SimbirGO_API.Models
{

    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }


        [Required]
        public string Role { get; set; }

        public DateTime RegistrationDate { get; set; }

        [Required]
        public string Password { get; set; }

        public string JwtToken { get; set; }

        [Required]
        public double Amount { get; set; }

        public List<Payment> PaymentHistory { get; set; }

    }
}
