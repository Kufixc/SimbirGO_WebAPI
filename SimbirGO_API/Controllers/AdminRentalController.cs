using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SimbirGO_API.Models;
using System.Data;

namespace SimbirGO_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class AdminRentalController : Controller
    {
        [HttpGet]
        public IEnumerable<Rental> GetRentals()
        {
            string query = "SELECT * FROM Rental";

            DataTable dataTable = DataBaseSource.WorkTable(query);

            List<Rental> rentals = new List<Rental>();

            foreach (DataRow row in dataTable.Rows)
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

            return rentals;
        }


        [HttpDelete("DeleteRental")]
        public IActionResult DeleteRental(int rentalId)
        {
            string deleteQuery = $"DELETE FROM Rental WHERE Id = {rentalId}";

            try
            {
                DataTable result = DataBaseSource.WorkTable($"SELECT COUNT(*) FROM Rental WHERE Id = {rentalId}");
                int rowCount = result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0][0]) : 0;

                if (rowCount > 0)
                {
                    DataBaseSource.Excet(deleteQuery);
                    return Ok("Аренда успешно удалена.");
                }
                else
                {
                    return BadRequest("Аренда с указанным ID не найден.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении аренды: {ex.Message}");
            }
        }

        [HttpPut("UpdateRental")]
        public IActionResult UpdateRental(Rental updatedRental)
        {
            try
            {
                if (updatedRental == null)
                {
                    return BadRequest("Неверный запрос. Обновляемый объект аренды не был предоставлен.");
                }

                string checkQuery = $"SELECT * FROM Rental WHERE Id = {updatedRental.Id}";
                DataTable checkResult = DataBaseSource.WorkTable(checkQuery);

                if (checkResult.Rows.Count == 0)
                {
                    return BadRequest("Аренда не найдена.");
                }

                int durationInMinutes = (updatedRental.DurationDays * 24 * 60) + (updatedRental.DurationHours * 60) + updatedRental.DurationMinutes;
               
                string updateQuery = $@" UPDATE Rental SET Name = '{updatedRental.Name}', Description = '{updatedRental.Description}', DurationDays = {updatedRental.DurationDays}, DurationHours = {updatedRental.DurationHours}, DurationMinutes = {updatedRental.DurationMinutes}, TotalCost = {updatedRental.TotalCost}, TransportId = {updatedRental.TransportId} WHERE Id = {updatedRental.Id}";

                DataBaseSource.Excet(updateQuery);

                string selectUpdatedQuery = $"SELECT * FROM Rental WHERE Id = {updatedRental.Id}";
                DataTable updatedResult = DataBaseSource.WorkTable(selectUpdatedQuery);

                if (updatedResult.Rows.Count == 0)
                {
                    return BadRequest("Аренда не найдена после обновления.");
                }

                Rental updatedRentalJson = new Rental
                {
                    Id = updatedRental.Id,
                    Name = updatedRental.Name,
                    Description = updatedRental.Description,
                    DurationDays = updatedRental.DurationDays,
                    DurationHours = updatedRental.DurationHours,
                    DurationMinutes = updatedRental.DurationMinutes,
                    TotalCost = updatedRental.TotalCost,
                    TransportId = updatedRental.TransportId
                };

                return Ok("Аренда успешно обновлена" + updatedRentalJson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении аренды: {ex.Message}");
            }
        }

        [HttpPost("CreateRental")]
        public IActionResult CreateRental(int rentalId, [FromBody] Rental newRental)
        {
            try
            {
                string checkQuery = $"SELECT * FROM Rental WHERE Id = {rentalId}";
                DataTable checkResult = DataBaseSource.WorkTable(checkQuery);

                if (checkResult.Rows.Count > 0)
                {
                    return BadRequest("Аренда с таким ID уже существует.");
                }

                string insertQuery = $@"INSERT INTO Rental (Name, Description, DurationDays, DurationHours, DurationMinutes, TotalCost, TransportId) VALUES ('{newRental.Name}', '{newRental.Description}', {newRental.DurationDays}, {newRental.DurationHours}, {newRental.DurationMinutes}, {newRental.TotalCost}, {newRental.TransportId})";

                DataBaseSource.Excet(insertQuery);

                return Ok("Аренда успешно добавлена.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при добавлении аренды: {ex.Message}");
            }
        }


    }
}
