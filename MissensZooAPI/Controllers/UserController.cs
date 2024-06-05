using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

namespace MissensZooAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=missensdatabas");

        public static Hashtable sessionId = new Hashtable();

        [HttpPost("CreateUser")]
        public ActionResult createAccount(User user) //Färdig
        {
            string checkUniqueUser = CheckIfUniqueUserDataExists(user);
            if (checkUniqueUser != String.Empty)
            {
                return BadRequest(checkUniqueUser);
            }

            try
            {
                connection.Open();
                string authorization = Request.Headers["Authorization"];

                MySqlCommand command = connection.CreateCommand();
                command.Prepare();

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
                command.CommandText = "INSERT INTO `user` (`role`, `userName`, `password`, `email`) VALUES ('1', @userName, @password, @eMail)";
                command.Parameters.AddWithValue("@userName", user.userName);
                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@eMail", user.eMail);


                int rows = command.ExecuteNonQuery();

                if (rows > 0)
                {
                    Guid guid = Guid.NewGuid();
                    string key = guid.ToString();
                    user.id = (int)command.LastInsertedId;
                    sessionId.Add(key, user);
                    connection.Close();
                    return StatusCode(201, key);

                }
            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
            return StatusCode(400);
        }

        [HttpGet("Login")]
        public ActionResult Login() //KOLLA IFALL REDAN INLOGGAD, 
        {
            string auth = this.HttpContext.Request.Headers["Authorization"];
            User user = DecodeUser(new User(), auth);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.Prepare();
            command.CommandText = "SELECT * FROM user WHERE email = @eMail";
            command.Parameters.AddWithValue("@eMail", user.eMail);
            MySqlDataReader data = command.ExecuteReader();
            try
            {

                string passwordHash = String.Empty;

                while (data.Read())
                {
                    passwordHash = data.GetString("password");
                    user.id = data.GetInt32("Id");
                    user.eMail = data.GetString("eMail");
                    user.role = data.GetInt32("role");
                }

                if (passwordHash != string.Empty && BCrypt.Net.BCrypt.Verify(user.password, passwordHash))
                {
                    Guid guid = Guid.NewGuid();
                    string key = guid.ToString();
                    Console.WriteLine(key);
                    sessionId.Add(key, user);
                    connection.Close();
                    return Ok(key);
                }

                connection.Close();
                return StatusCode(400);
            }
            catch (Exception exception)
            {
                connection.Close();
                Console.WriteLine($"Login failed: {exception.Message}");
                return StatusCode(500);
            }
        }

        [HttpGet("VerifyRole")]
        public ActionResult VerifyRole()
        {
            string auth = this.HttpContext.Request.Headers["Authorization"];

            if (auth == null || !UserController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "0");
            }
            User user = (User)UserController.sessionId[auth];
            return StatusCode(200, user.role);
<<<<<<< HEAD
        }

        [HttpGet("VerifyUserId")]
        public ActionResult VerifyUserId()
        {
            string auth = this.HttpContext.Request.Headers["Authorization"];
            if (auth == null || !UserController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "0");
            }
            User user = (User)UserController.sessionId[auth];
            return StatusCode(200, user.id);
=======
>>>>>>> a561ee47af56a8ff22f1f71b1b984e1ffdd23413
        }

        [HttpGet("VerifyUserId")]
        public ActionResult VerifyUserId()
        {
            string auth = this.HttpContext.Request.Headers["Authorization"];
            if (auth == null || !UserController.sessionId.ContainsKey(auth))
            {
                return StatusCode(403, "0");
            }
            User user = (User)UserController.sessionId[auth];
            return StatusCode(200, user.id);
        }




        [HttpPut("UpdatePassword")]
        public ActionResult ChangePassword(User user) //CHECK ADMIN ROLE
        {
            string auth = this.HttpContext.Request.Headers["Authorization"];

            connection.Open();
            try
            {
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "UPDATE `user` SET `password` = @password WHERE `id` = @id";

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);

                command.Parameters.AddWithValue("@password", hashedPassword);
                command.Parameters.AddWithValue("@id", user.id);

                MySqlDataReader data = command.ExecuteReader();

                connection.Close();
                return StatusCode(200, "Lyckades uppdatera lösenord");
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }

        [HttpPut("UpdateUserRole")] //NEEDS TO CHECK IF USER IS ADMIN BEFORE PERMISSION TO CHANGE
        public ActionResult UpdateUserRole(User user)
        {
            string auth = this.HttpContext.Request.Headers["Authorization"];

            
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "UPDATE `user` SET `role` = @role WHERE `id` = @id";

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);

                command.Parameters.AddWithValue("@role", user.role); //1 Guest access, 2 Regular user access, 3 Admin access
                command.Parameters.AddWithValue("@id", user.id);

                MySqlDataReader data = command.ExecuteReader();

                connection.Close();
                return StatusCode(200, $"Lyckades uppdatera användare till {user.role}");
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }

        [HttpDelete("RemoveUser")] //NEEDS TO CHECK IF USER IS ADMIN BEFORE PERMISSION TO CHANGE
        public ActionResult RemoveUser(User user)
        {
            string auth = this.HttpContext.Request.Headers["Authorization"];
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "DELETE FROM `user` where `id` = @id";
                command.Parameters.AddWithValue("@id", user.id);

                MySqlDataReader data = command.ExecuteReader();

                connection.Close();
                return StatusCode(200, $"Lyckades ta bort användare {user.id}");
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }

        private string CheckIfUniqueUserDataExists(User user)
        {
            string checkUniqueUser = String.Empty;
            try
            {
                connection.Open();
                MySqlCommand query = connection.CreateCommand();
                query.Prepare();
                query.CommandText = "SELECT * FROM user WHERE Email = @userEmail OR userName = @userName";
                query.Parameters.AddWithValue("@userName", user.userName);
                query.Parameters.AddWithValue("@userEmail", user.eMail);
                MySqlDataReader data = query.ExecuteReader();

                if (data.Read())
                {
                    if (data.GetString("Email") == user.eMail)
                    {
                        checkUniqueUser = "Email används redan på hemsidan";
                    }
                    if (data.GetString("userName") == user.userName)
                    {
                        checkUniqueUser = "Användarnamn används redan på hemsidan";
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"UserController.CheckIfUniqueUserDataExists: {exception.Message}");
                connection.Close();
            }

            return checkUniqueUser;
        }

        private User DecodeUser(User user, string auth)
        {
            if (auth != null && auth.StartsWith("Basic"))
            {
                string encodedUsernamePassword = auth.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                user.eMail = usernamePassword.Substring(0, seperatorIndex);
                user.password = usernamePassword.Substring(seperatorIndex + 1);
            }
            else
            {
                //Handle what happens if that isn't the case
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }
            return user;
        }
    }
}
