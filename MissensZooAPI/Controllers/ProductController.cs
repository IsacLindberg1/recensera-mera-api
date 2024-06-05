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

    public class ProductController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=missensdatabas");

        public static Hashtable sessionId = new Hashtable();

        [HttpPost("CreateProduct")]
        public ActionResult CreateProduct(Product product) //CHECK USER ROLE
        {
            try
            {
                connection.Open();
                string userHeader = Request.Headers["Authorization"];
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "INSERT INTO `product` (`description`, `name`, `category`, `price`, `stock`, `image`, `portionPerDay`, `foodInfo`, `onSale`, `salePercentage`) VALUES(@description, @name, @category, @price, @inStock, @image, @portionPerDay, @foodInfo,  @onSale, @salesPercentage)";
                command.Parameters.AddWithValue("@description", product.description);
                command.Parameters.AddWithValue("@name", product.name);
                command.Parameters.AddWithValue("@category", product.category);
                command.Parameters.AddWithValue("@price", product.price);
                command.Parameters.AddWithValue("@inStock", product.inStock);
                command.Parameters.AddWithValue("@image", product.image);
                command.Parameters.AddWithValue("@portionPerDay", product.portionPerDay);
                command.Parameters.AddWithValue("@foodInfo", product.portionPerDay);
                command.Parameters.AddWithValue("@onSale", product.onSale);
                command.Parameters.AddWithValue("@salesPercentage", product.salesPercentage);

                int rows = command.ExecuteNonQuery();
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
            connection.Close();
            return StatusCode(201, $"Lyckades skapa produkt med namn: {product.name}");

        }

        [HttpGet("ViewAllProducts")]
        public ActionResult<List<Product>> ViewAllProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "SELECT * FROM `product`";
                MySqlDataReader data = command.ExecuteReader();

                while (data.Read())
                {
                    Product product = new Product
                    {
                        name = data.GetString("name"),
                        price = data.GetInt32("price"),
                        description = data.GetString("description"),
                        category = data.GetString("category"),
                        inStock = data.GetInt32("stock"),
                        image = data.GetString("image"),
                        rating = data.GetInt32("rating"),
                        ratingCount = data.GetInt32("ratingCount"),
                        portionPerDay = data.GetInt32("portionPerDay"),
                        onSale = data.GetInt32("onSale"), //Hur ska detta bli boolean???????
                        salesPercentage = data.GetInt32("salePercentage"),
                        id = data.GetInt32("id"),
                    };
                    products.Add(product);
                }
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
            connection.Close();
            return StatusCode(200, products);         
        }


        [HttpPut("ChangeProduct")] //CHECK ADMIN ROLE
        public ActionResult ChangeProduct(Product product)
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "UPDATE `product` SET `description` = @description, `name` = @name, `category` = @category, `price` = @price, `stock` = @stock, `image` = @image, `portionPerDay` " +
                    "= @portionPerDay, `foodInfo` = @foodInfo, `onSale` = @onSale, `salePercentage` = @salesPercentage WHERE `id` = @id";

                command.Parameters.AddWithValue("@name", product.name);
                command.Parameters.AddWithValue("@price", product.price);
                command.Parameters.AddWithValue("@description", product.description);
                command.Parameters.AddWithValue("@category", product.category);
                command.Parameters.AddWithValue("@stock", product.inStock);
                command.Parameters.AddWithValue("@image", product.image);
                command.Parameters.AddWithValue("@portionPerDay", product.portionPerDay);
                command.Parameters.AddWithValue("@foodInfo", product.foodInfo);
                command.Parameters.AddWithValue("@onSale", product.onSale);
                command.Parameters.AddWithValue("@salesPercentage", product.salesPercentage);
                command.Parameters.AddWithValue("@id", product.id);


                command.ExecuteNonQuery();

                connection.Close();
                return StatusCode(200, $"Product med id = {product.id} ändrades");
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }

        }

        [HttpDelete("RemoveProduct")]
        public ActionResult RemoveProduct(Product product)
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "DELETE FROM `product` where `id` = @id";
                command.Parameters.AddWithValue("@id", product.id);
                command.ExecuteNonQuery();

                connection.Close();
                return StatusCode(200, $"Lyckades ta bort product med id = {product.id}");
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }
    }
}
