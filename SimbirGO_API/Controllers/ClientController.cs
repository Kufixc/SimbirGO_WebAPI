using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimbirGO_API.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SimbirGO_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class ClientController : Controller
    {
        private static string GenerateJWT(Client client)
        {
            var key = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserName", client.UserName),
                    new Claim("Email", client.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return jwtToken;
        }
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return Regex.IsMatch(email, emailPattern);
        }

        #region Http запросы


        [HttpPost("login")]
        public IActionResult Authenticate(string username, string password)
        {
            string query = $"SELECT * FROM Client WHERE Name = '{username}' AND Password = '{password}'";
            DataTable result = DataBaseSource.WorkTable(query);

            if (result.Rows.Count == 0)
            {
                return Unauthorized("Пользователь с таким именем и паролем не существует.");
            }

            return Ok();
        }


        [HttpPost("registration")]
        public IActionResult Registration(string username, string email, string phone, string password)
        {

            if (!IsValidEmail(email))
            {
                return BadRequest("Некорректный формат электронной почты.");
            }

            string query = $"SELECT * FROM Client WHERE Name = '{username}' OR Email = '{email}' OR Phone = '{phone}'";
            DataTable result = DataBaseSource.WorkTable(query);



            if (result.Rows.Count > 0)
            {
                return BadRequest("Пользователь с таким именем, электронной почтой или номером телефона уже существует.");
            }

            var newClient = new Client
            {
                UserName = username,
                Email = email,
                Phone = phone,
                Password = password,
                RegistrationDate = DateTime.Now,
                Role = "user",
                Amount = 0,
            };

        
            string insertQuery = $"INSERT INTO Client (Name, Email, Phone, Password, RegistrationDate, Role, JwtToken, Amount) " + $"VALUES ('{newClient.UserName}', '{newClient.Email}', '{newClient.Phone}', '{newClient.Password}', " + $"'{newClient.RegistrationDate}', '{newClient.Role}', 0)";

            DataBaseSource.Excet(insertQuery);

            return Ok("Успешная регистрация");
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateClient([FromBody] Client newClient)
        {
            string query = $"SELECT * FROM Client WHERE Id = {newClient.Id}";
            DataTable result = DataBaseSource.WorkTable(query);

            if (result.Rows.Count == 0)
            {
                return BadRequest("Учетная запись не найдена.");
            }

            string updateQuery = $"UPDATE Client " + $"SET Name = '{newClient.UserName}', " + $"Email = '{newClient.Email}', " + $"Phone = '{newClient.Phone}', " + $"Password = '{newClient.Password}', " + $"Role = '{newClient.Role}' " + $"WHERE Id = {newClient.Id}";

            DataBaseSource.Excet(updateQuery);

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


        //Мы можем для пользователя устанавливать его ID и он будет удалять свой аккаунт
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

        #endregion

    }
}
