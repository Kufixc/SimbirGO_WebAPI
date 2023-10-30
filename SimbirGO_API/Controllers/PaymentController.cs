using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirGO_API.Models;
using System.Data;

namespace SimbirGO_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class PaymentController : Controller
    {

        [HttpGet("GetPayment")]
        public IActionResult GetPayment(int idClient, int idPayment)
        {
            if (idClient < 0 || idPayment < 0)
            {
                return NotFound("Клиент или платеж не найден");
            }

            string query = $"SELECT * FROM Payment WHERE ClientId = {idClient} AND Id = {idPayment}";

            DataTable paymentTable = DataBaseSource.WorkTable(query);

            if (paymentTable.Rows.Count == 0)
            {
                return NotFound("Клиент или платеж не найден");
            }

            var payment = new Payment
            {
                Id = Convert.ToInt32(paymentTable.Rows[0]["Id"]),
                Name = paymentTable.Rows[0]["Name"].ToString(),
                Description = paymentTable.Rows[0]["Description"].ToString(),
                Cost = Convert.ToDouble(paymentTable.Rows[0]["Cost"]),
                DateOperazion = Convert.ToDateTime(paymentTable.Rows[0]["DateOperazion"]),
                ClientID = Convert.ToInt32(paymentTable.Rows[0]["ClientId"])
            };

            return Ok(payment);
        }



        [HttpGet("GetPaymentHistory")]
        public IActionResult GetPaymentHistory(int idClient)
        {
            if (idClient < 0)
            {
                return NotFound("Id клиента не найден");
            }

            string query = $"SELECT * FROM Payment WHERE ClientId = {idClient}";

            DataTable paymentTable = DataBaseSource.WorkTable(query);

            List<Payment> paymentHistory = new List<Payment>();

            foreach (DataRow row in paymentTable.Rows)
            {
                var payment = new Payment
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Cost = Convert.ToDouble(row["Cost"]),
                    DateOperazion = Convert.ToDateTime(row["DateOperazion"]),
                    ClientID = Convert.ToInt32(row["ClientId"])
                };

                paymentHistory.Add(payment);
            }

            return Ok(paymentHistory);
        }


        [HttpPost("AddPayment")]
        public IActionResult AddPayment(int idClient, [FromBody] Payment payment)
        {
            if (idClient < 0)
            {
                return NotFound("Клиент не найден");
            }

            string addPaymentQuery = $"INSERT INTO Payment (Name, Description, Cost, DateOperazion, ClientId) " +
                $"VALUES ('{payment.Name}', '{payment.Description}', {payment.Cost}, current_timestamp, {idClient})";

            try
            {
                DataBaseSource.Excet(addPaymentQuery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при добавлении платежа: {ex.Message}");
            }

            string updateBalanceQuery = $"UPDATE Client SET Amount = Amount - {payment.Cost} WHERE Id = {idClient}";

            try
            {
                DataBaseSource.Excet(updateBalanceQuery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении баланса клиента: {ex.Message}");
            }

            return Ok("Операция/оплата успешно добавлена");
        }


        [HttpPost("RefillAccount")]
        public IActionResult RefillAccount(int idClient, double amount)
        {
            string checkClientQuery = $"SELECT COUNT(*) FROM Client WHERE Id = {idClient}";
            DataTable clientResult = DataBaseSource.WorkTable(checkClientQuery);
            int clientCount = clientResult.Rows.Count > 0 ? Convert.ToInt32(clientResult.Rows[0][0]) : 0;

            if (clientCount == 0)
            {
                return NotFound("Клиент не найден");
            }

            string addPaymentQuery = $"INSERT INTO Payment (Name, Description, Cost, DateOperazion, ClientId) " +
                $"VALUES ('Пополнение счета', 'Пополнение баланса клиента', {amount}, current_timestamp, {idClient})";

            try
            {
                DataBaseSource.Excet(addPaymentQuery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при добавлении платежа: {ex.Message}");
            }

            string updateBalanceQuery = $"UPDATE Client SET Amount = Amount + {amount} WHERE Id = {idClient}";

            try
            {
                DataBaseSource.Excet(updateBalanceQuery);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении баланса клиента: {ex.Message}");
            }

            return Ok("Пополнение счета успешно выполнено");
        }
    }
}
