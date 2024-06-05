using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Buffers.Text;
using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace MissensZooAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class BlogController : Controller
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;uid=root;pwd=;database=missensdatabas");
       
        [HttpPost("UploadImage")]
        public ActionResult Upload(Blog blog)
        {
            const string DIRECTORY = "C:\\Users\\Elev\\Documents\\GitHub\\MissensZooOchBlogg\\images";
            blog.image = blog.image.Split(',')[1];
            byte[] data = Convert.FromBase64String(blog.image);
            string path = DIRECTORY + blog.image + ".png";

            try
            {
                System.IO.File.WriteAllBytes(path, data);
            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }

            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "insert into blogpost(image) values(@image)";
                cmd.Parameters.AddWithValue("@image", path);

                int rows = cmd.ExecuteNonQuery();

                if(rows == 0)
                {
                    System.IO.File.Delete(path);
                    connection.Close();
                    return StatusCode(500, "Image rows was zero");
                }
            }
            catch (Exception exception)
            {
                System.IO.File.Delete(path);
                connection.Close();
                return StatusCode(500,exception.Message);
                
            }
            connection.Close();
            return StatusCode(200, $"Successfully uploaded image as {path}");
        }

        [HttpPost("CreateBlog")]
        public ActionResult CreateBlog(Blog blog) //CHECK USER ROLE
        {
            User user = new User();
            try
            {
                connection.Open();
                string userHeader = Request.Headers["Authorization"];
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();

                command.CommandText = "INSERT INTO `blogpost` (`title`, `userId`, `blogContent`, `image`, `timestamp`, `category`) VALUES (@title, @userId, @blogContent, @image, (SELECT CURRENT_TIMESTAMP), @category)";
                command.Parameters.AddWithValue("@title", blog.title);
                command.Parameters.AddWithValue("@userId", user.id);
                command.Parameters.AddWithValue("@blogContent", blog.blogContent);
                command.Parameters.AddWithValue("@image", blog.image);
                command.Parameters.AddWithValue("@category", blog.category);
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
            connection.Close();
            return StatusCode(201, "lyckades skapa Blog");
        }
        
        [HttpPost("CreateComment")]
        public ActionResult CreateComment(Comment comment)
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "INSERT INTO `comments` (`blogId` = @blogId, `userId` = @userId, `content` = @content, `Timestamp` = (SELECT CURRENT_TIMESTAMP)) WHERE `id`, = @id";
                command.ExecuteNonQuery();

                connection.Close();
                return StatusCode(200, $"Lyckades skapa kommentar på blog = {comment.blogId} Content = {comment.content}");
            }
            catch (Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }
        
        [HttpGet("ViewAllBlogs")]
        public ActionResult<List<Blog>> ViewAllBlogs()
        {
            List<Blog> blogs = new List<Blog>();
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "SELECT `blogId`, `title`, `userId`, `content`, `image`, `timestamp`, `category`, `userName` FROM `blogpost` t1 LEFT JOIN  `user` t2 ON t1.userId = t2.id;";
                MySqlDataReader data = command.ExecuteReader();

                while (data.Read())
                {
                    Blog blog = new Blog();
                    blog.blogId = data.GetInt32("blogId");
                    blog.title = data.GetString("title");
                    blog.blogContent = data.GetString("content");
                    blog.username = data.GetString("userName");
                    blog.image = data.GetString("image");
                    blog.timestamp = data.GetString("timestamp");
                    blog.category = data.GetString("category");

                    blogs.Add(blog);
                }
            }
            catch(Exception exception)
            {
                connection.Close();
                return StatusCode(500, exception.Message);
                //Will give error if blog has the id of user that does not exist.
            }
            connection.Close();
            return Ok(blogs);
        }

        [HttpDelete("RemoveBlog")]
        public ActionResult RemoveBlog(Blog blog) //NEEDS TO CHECK IF ADMIN
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Prepare();
                command.CommandText = "DELETE FROM `blogpost` where `id` = @id";
                command.Parameters.AddWithValue("@id", blog.blogId);
                command.ExecuteNonQuery();

                connection.Close();
                return StatusCode(200, $"Lyckades ta bort blog med id: {blog.blogId}");
            }
            catch(Exception exception ) 
            {
                connection.Close();
                return StatusCode(500, exception.Message);
            }
        }
    }
}
