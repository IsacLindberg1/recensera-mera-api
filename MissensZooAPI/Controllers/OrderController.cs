using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MissensZooAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OrderController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=missensdatabas");

        [HttpPost("CreateOrder")]
        public ActionResult CreateOrder(Order order)
        {
            try
            {
                connection.Open();
                string userHeader = Request.Headers["Authorization"];
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();

                command.CommandText = "INSERT INTO `orders` (`userId`, `price`, `phonenumber`, `zipcode`, `country`, `city`, `adress`, recipientName ) VALUES(@userId, @price, @phonenumber, @zipcode, @country, @city, @adress, @recipientName)";
                command.Parameters.AddWithValue("@userId", order.userId);
                command.Parameters.AddWithValue("@price", order.orderPrice);
                command.Parameters.AddWithValue("@phonenumber", order.phonenumber);
                command.Parameters.AddWithValue("@zipcode", order.zipcode);
                command.Parameters.AddWithValue("@country", order.country);
                command.Parameters.AddWithValue("@city", order.city);
                command.Parameters.AddWithValue("@adress", order.adress);
                command.Parameters.AddWithValue("@recipientName", order.recipientName);

                MySqlDataReader data = command.ExecuteReader();
                connection.Close();
                CreateProductsForOrder(order);
                ClearCartAfterOrder(order);
                return StatusCode(201, $"Lyckades skapa order med användarid: {order.Id}");

            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }

        [HttpPost("CreateProductsForOrder")]
        public ActionResult CreateProductsForOrder(Order order)
        {
            try
            {
                connection.Open();
                string userHeader = Request.Headers["Authorization"];
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "INSERT INTO `orderproducts` (`userId`, `orderproducts`.`productId`, `orderproducts`.`quantity`, `orderproducts`.`price`) SELECT @userId, `cart`.`productId`, `cart`.`quantity`, `cart`.`price` FROM `cart` LEFT JOIN `orderproducts` ON `orderproducts`.`userId` = `cart`.`userId` WHERE `cart`.`userId` = @userId";
                command.Parameters.AddWithValue("@userId", order.userId);

                MySqlDataReader data = command.ExecuteReader();
                connection.Close();
                return StatusCode(201, $"Lyckades lägga till producter från cart där användarid är: {order.userId}");
            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }

        [HttpDelete("ClearCartAfterOrder")]
        public ActionResult ClearCartAfterOrder(Order order)
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "DELETE FROM `cart` WHERE `userId` = @userId";
                command.Parameters.AddWithValue("@userId", order.userId);
                MySqlDataReader data = command.ExecuteReader();

                connection.Close();
                return StatusCode(200, $"Lyckades ta bort innehåll i kundvagn där userId: {order.userId}");
            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }
    }
}
