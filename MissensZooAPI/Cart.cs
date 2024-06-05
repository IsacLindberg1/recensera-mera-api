using MissensZooAPI.Controllers;


namespace MissensZooAPI
{
    public class Cart
    {
        public int userId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public int price { get; set; }
    }
}
