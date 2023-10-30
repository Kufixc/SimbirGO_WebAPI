using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirGO_API.Models;
using System.Data;

namespace SimbirGO_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class AdminTransportController : Controller
    {

        [HttpGet("GetTransports")]
        public IEnumerable<Transport> GetTransports()
        {
            string query = "SELECT * FROM Transport";

            DataTable dataTable = DataBaseSource.WorkTable(query);

            List<Transport> transports = new List<Transport>();

            foreach (DataRow row in dataTable.Rows)
            {
                Transport transport = new Transport
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Type = row["Type"].ToString(),
                    Speed = Convert.ToDouble(row["Speed"]),
                    RentPrice = Convert.ToDouble(row["RentPrice"]),
                    Color = row["Color"].ToString(),
                    Availability = Convert.ToBoolean(row["Availability"])
                };

                transports.Add(transport);
            }

            return transports;
        }



        [HttpPut("UpdateTransport")]
        public IActionResult UpdateTransport([FromBody] Transport updatedTransport)
        {
            try
            {
                string checkQuery = $"SELECT * FROM Transport WHERE Id = {updatedTransport.Id}";
                DataTable checkResult = DataBaseSource.WorkTable(checkQuery);

                if (checkResult.Rows.Count == 0)
                {
                    return BadRequest("Транспорт не найден.");
                }

                string updateQuery = $@"
            UPDATE Transport 
            SET 
                Type = '{updatedTransport.Type}', 
                Speed = {updatedTransport.Speed}, 
                RentPrice = {updatedTransport.RentPrice}, 
                Color = '{updatedTransport.Color}', 
                Availability = {updatedTransport.Availability} 
            WHERE Id = {updatedTransport.Id}";

                DataBaseSource.Excet(updateQuery);

                string selectUpdatedQuery = $"SELECT * FROM Transport WHERE Id = {updatedTransport.Id}";
                DataTable updatedResult = DataBaseSource.WorkTable(selectUpdatedQuery);

                if (updatedResult.Rows.Count == 0)
                {
                    return BadRequest("Транспорт не найден после обновления.");
                }

                Transport updatedTransportFromDB = new Transport
                {
                    Id = Convert.ToInt32(updatedResult.Rows[0]["Id"]),
                    Type = updatedResult.Rows[0]["Type"].ToString(),
                    Speed = Convert.ToDouble(updatedResult.Rows[0]["Speed"]),
                    RentPrice = Convert.ToDouble(updatedResult.Rows[0]["RentPrice"]),
                    Color = updatedResult.Rows[0]["Color"].ToString(),
                    Availability = Convert.ToBoolean(updatedResult.Rows[0]["Availability"])
                };

                return Ok(updatedTransportFromDB);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении транспорта: {ex.Message}");
            }
        }


        [HttpPost("AddTransport")]
        public IActionResult AddTransport([FromBody] Transport newTransport)
        {
            string checkQuery = $"SELECT * FROM Transport WHERE Id = {newTransport.Id}";
            DataTable checkResult = DataBaseSource.WorkTable(checkQuery);

            if (checkResult.Rows.Count > 0)
            {
                return BadRequest("Транспорт с таким ID уже существует.");
            }

            string insertQuery = $@"
        INSERT INTO Transport (Type, Speed, RentPrice, Color, Availability)
        VALUES (
            '{newTransport.Type}',
            {newTransport.Speed},
            {newTransport.RentPrice},
            '{newTransport.Color}',
            {newTransport.Availability}
        )";

            try
            {
                DataBaseSource.Excet(insertQuery);
                return Ok("Транспорт успешно добавлен.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при добавлении транспорта: {ex.Message}");
            }
        }


        [HttpDelete("DeleteTransport")]
        public IActionResult Delete(int id)
        {
            string deleteQuery = $"DELETE FROM Transport WHERE Id = {id}";

            try
            {
                DataTable result = DataBaseSource.WorkTable($"SELECT COUNT(*) FROM Transport WHERE Id = {id}");
                int rowCount = result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0][0]) : 0;

                if (rowCount > 0)
                {
                    DataBaseSource.Excet(deleteQuery);
                    return Ok("Транспорт успешно удален.");
                }
                else
                {
                    return BadRequest("Транспорт с указанным ID не найден.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении транспорта: {ex.Message}");
            }
        }

    }
}
