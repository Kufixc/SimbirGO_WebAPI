using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirGO_API.Models;
using System.Data;

namespace SimbirGO_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class TransportController : Controller
    {
        [HttpGet("GetTransport")]
        public IActionResult GetTransport(int transportId)
        {
            string query = $"SELECT * FROM Transport WHERE Id = {transportId}";
            DataTable transportTable = DataBaseSource.WorkTable(query);

            if (transportTable.Rows.Count == 0)
            {
                return NotFound("Транспорт не найден");
            }

            var row = transportTable.Rows[0];

            var transport = new Transport
            {
                Id = Convert.ToInt32(row["Id"]),
                Type = row["Type"].ToString(),
                Speed = Convert.ToDouble(row["Speed"]),
                RentPrice = Convert.ToDouble(row["RentPrice"]),
                Color = row["Color"].ToString(),
                Availability = Convert.ToBoolean(row["Availability"])
            };

            return Ok(transport);
        }

        [HttpGet("GetTransports")]
        public IActionResult GetTransports()
        {
            string query = "SELECT * FROM Transport";
            DataTable transportTable = DataBaseSource.WorkTable(query);

            List<Transport> transports = new List<Transport>();

            foreach (DataRow row in transportTable.Rows)
            {
                var transport = new Transport
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

            return Ok(transports);
        }

    }
}
