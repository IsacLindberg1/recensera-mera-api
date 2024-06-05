using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MissensZooAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class CartController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=missensdatabas");


        [HttpPost("AddProductToCart")]
        public ActionResult AddProductToCart(Cart cart) //CHECK USER ROLE
        {
            try
            {
                connection.Open();
                string userHeader = Request.Headers["Authorization"];
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();

                command.CommandText = "INSERT INTO `cart` (`userId`, `productId`, `quantity`) VALUES(@userId, @productId, @quantity)";
                command.Parameters.AddWithValue("@userId", cart.userId);
                command.Parameters.AddWithValue("@productId", cart.productId);
                command.Parameters.AddWithValue("quantity", cart.quantity);

                MySqlDataReader data = command.ExecuteReader();

                connection.Close();
                return StatusCode(201, "Lyckades lägga till product till kundvagn");
            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }


        [HttpGet("ViewCart/{userId}")]
        public ActionResult<List<Cart>> ViewCart(int userId) //CHECK USER ROLE
        {
            List<Cart> carts = new List<Cart>();
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "SELECT t1.`id`, t1.`userId`, t1.`productId`, t1.`quantity`, t1.`price` FROM `cart` t1 LEFT JOIN `product` t2 ON t1.productId = t2.id WHERE `userId` = @userId;";
                command.Parameters.AddWithValue("@userId", userId);
                MySqlDataReader data = command.ExecuteReader();

                while (data.Read())
                {
                    Cart cart = new Cart();

                    cart.userId = data.GetInt32("userId");
                    cart.productId = data.GetInt32("productId");
                    cart.quantity = data.GetInt32("quantity");
                    cart.price = data.GetInt32("price");
                    carts.Add(cart);
                }
                return Ok(carts);
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception);
            }
        }
    }
}
