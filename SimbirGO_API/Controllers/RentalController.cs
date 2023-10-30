using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirGO_API.Models;
using System.Data;

namespace SimbirGO_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class RentalController : Controller
    {


        [HttpGet("GetRental")]
        public IActionResult GetRental(int rentalId)
        {
            string query = $"SELECT * FROM Rental WHERE Id = {rentalId}";
            DataTable rentalTable = DataBaseSource.WorkTable(query);

            if (rentalTable.Rows.Count == 0)
            {
                return NotFound("Аренда не найдена");
            }

            Rental rental = new Rental
            {
                Id = Convert.ToInt32(rentalTable.Rows[0]["Id"]),
                Name = rentalTable.Rows[0]["Name"].ToString(),
                Description = rentalTable.Rows[0]["Description"].ToString(),
                DurationDays = Convert.ToInt32(rentalTable.Rows[0]["DurationDays"]),
                DurationHours = Convert.ToInt32(rentalTable.Rows[0]["DurationHours"]),
                DurationMinutes = Convert.ToInt32(rentalTable.Rows[0]["DurationMinutes"]),
                TotalCost = Convert.ToDouble(rentalTable.Rows[0]["TotalCost"]),
                TransportId = Convert.ToInt32(rentalTable.Rows[0]["TransportId"]),
            };

            return Ok(rental);
        }

        [HttpGet("getRentals")]
        public IActionResult GetRentals()
        {
            string query = "SELECT * FROM Rental";
            DataTable rentalTable = DataBaseSource.WorkTable(query);

            List<Rental> rentals = new List<Rental>();

            foreach (DataRow row in rentalTable.Rows)
            {
                Rental rental = new Rental
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    DurationDays = Convert.ToInt32(row["DurationDays"]),
                    DurationHours = Convert.ToInt32(row["DurationHours"]),
                    DurationMinutes = Convert.ToInt32(row["DurationMinutes"]),
                    TotalCost = Convert.ToDouble(row["TotalCost"]),
                    TransportId = Convert.ToInt32(row["TransportId"]),
                };

                rentals.Add(rental);
            }

            return Ok(rentals);
        }
    }
}
