using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimbirGO_API.Models;
using System.Data;

namespace SimbirGO_API.Controllers
{
    [Authorize]
    [Route("/api/[controller]")]
    public class AdminContoller : Controller
    {
        [HttpGet]
        public IEnumerable<Client> GetClients()
        {
            string clientQuery = "SELECT * FROM Client";
            DataTable clientTable = DataBaseSource.WorkTable(clientQuery);

            List<Client> clients = new List<Client>();

            foreach (DataRow clientRow in clientTable.Rows)
            {
                var client = new Client
                {
                    Id = Convert.ToInt32(clientRow["Id"]),
                    UserName = clientRow["UserName"].ToString(),
                    Email = clientRow["Email"].ToString(),
                    Phone = clientRow["Phone"].ToString(),
                    Role = clientRow["Role"].ToString(),
                    RegistrationDate = Convert.ToDateTime(clientRow["RegistrationDate"]),
                    Password = clientRow["Password"].ToString(),

                    Amount = Convert.ToDouble(clientRow["Amount"])
                };

                string paymentQuery = $"SELECT * FROM Payment WHERE ClientId = {client.Id}";
                DataTable paymentTable = DataBaseSource.WorkTable(paymentQuery);

                List<Payment> paymentHistory = new List<Payment>();
                foreach (DataRow paymentRow in paymentTable.Rows)
                {
                    var payment = new Payment
                    {
                        Id = Convert.ToInt32(paymentRow["Id"]),
                        Name = paymentRow["Name"].ToString(),
                        Description = paymentRow["Description"].ToString(),
                        Cost = Convert.ToDouble(paymentRow["Cost"]),
                        DateOperazion = Convert.ToDateTime(paymentRow["DateOperazion"]),
                        ClientID = Convert.ToInt32(paymentRow["ClientId"])
                    };
                    paymentHistory.Add(payment);
                }

                client.PaymentHistory = paymentHistory;

                clients.Add(client);
            }

            return clients;
        }


        [HttpDelete("DeleteAccaunt")]
        public IActionResult Delete(int id)
        {
            string deleteQuery = $"DELETE FROM Client WHERE Id = {id}";

            try
            {
                DataTable result = DataBaseSource.WorkTable($"SELECT COUNT(*) FROM Client WHERE Id = {id}");
                int rowCount = result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0][0]) : 0;

                if (rowCount > 0)
                {
                    DataBaseSource.Excet(deleteQuery);
                    return Ok("Пользователь успешно удален.");
                }
                else
                {
                    return BadRequest("Пользователь с указанным ID не найден.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении пользователя: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateClient([FromBody] Client newClient)
        {
            try
            {
               //Операция над данными в базе
                string checkQuery = $"SELECT * FROM Client WHERE Id = {newClient.Id}";
                DataTable checkResult = DataBaseSource.WorkTable(checkQuery);

                if (checkResult.Rows.Count == 0)
                {
                    return BadRequest("Учетная запись не найдена.");
                }

                string updateQuery = $"UPDATE Client SET " + $"Name = '{newClient.UserName}', " + $"Email = '{newClient.Email}', " + $"Phone = '{newClient.Phone}', " + $"Password = '{newClient.Password}', " + $"Role = '{newClient.Role}' " + $"WHERE Id = {newClient.Id}";

             
                DataBaseSource.Excet(updateQuery);

                //Тут мы получаем json файл в HTTP запроса
                string selectUpdatedQuery = $"SELECT * FROM Client WHERE Id = {newClient.Id}";
                DataTable updatedResult = DataBaseSource.WorkTable(selectUpdatedQuery);

                var updatedClient = new Client
                {
                    Id = Convert.ToInt32(updatedResult.Rows[0]["Id"]),
                    UserName = updatedResult.Rows[0]["UserName"].ToString(),
                    Email = updatedResult.Rows[0]["Email"].ToString(),
                    Phone = updatedResult.Rows[0]["Phone"].ToString(),
                    Role = updatedResult.Rows[0]["Role"].ToString(),
                    RegistrationDate = Convert.ToDateTime(updatedResult.Rows[0]["RegistrationDate"]),
                    Password = updatedResult.Rows[0]["Password"].ToString(),
                    Amount = Convert.ToDouble(updatedResult.Rows[0]["Amount"])
                };

                return Ok(updatedClient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при обновлении пользователя: {ex.Message}");
            }
        }



        [HttpGet("GetId")]
        public IActionResult Get(int id)
        {
            try
            {
                string query = $"SELECT * FROM Client WHERE Id = {id}";
                DataTable result = DataBaseSource.WorkTable(query);

                if (result.Rows.Count > 0)
                {
                    var client = new Client
                    {
                        Id = Convert.ToInt32(result.Rows[0]["Id"]),
                        UserName = result.Rows[0]["UserName"].ToString(),
                        Email = result.Rows[0]["Email"].ToString(),
                        Phone = result.Rows[0]["Phone"].ToString(),
                        Role = result.Rows[0]["Role"].ToString(),
                        RegistrationDate = Convert.ToDateTime(result.Rows[0]["RegistrationDate"]),
                        Password = result.Rows[0]["Password"].ToString(),
                        Amount = Convert.ToDouble(result.Rows[0]["Amount"])
                    };
                    
                    string paymentQuery = $"SELECT * FROM Payment WHERE ClientId = {id}";
                    DataTable paymentResult = DataBaseSource.WorkTable(paymentQuery);

                    List<Payment> paymentHistory = new List<Payment>();
                    foreach (DataRow row in paymentResult.Rows)
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

                    client.PaymentHistory = paymentHistory;

                    return Ok(client);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении пользователя: {ex.Message}");
            }
        }
    }
}
