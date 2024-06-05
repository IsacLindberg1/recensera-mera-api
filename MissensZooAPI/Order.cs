using MissensZooAPI.Controllers;

namespace MissensZooAPI
{
    public class Order
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int orderPrice { get; set; }
        public string phonenumber { get; set; } = string.Empty;
        public string zipcode { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;
        public string city { get; set; } = string.Empty;
        public string adress { get; set; } = string.Empty;
        public string recipientName { get; set; } = string.Empty;
    }
}
