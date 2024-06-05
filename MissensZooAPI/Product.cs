using MissensZooAPI.Controllers;

namespace MissensZooAPI
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public int price { get; set; }
        public string description { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public int inStock { get; set; }

        public string image { get; set; } = string.Empty;

        public int rating { get; set; }
        public int ratingCount { get; set; }

        public int portionPerDay { get; set; }
        public string foodInfo { get; set; } = string.Empty;

        public int onSale { get; set; }
        public int salesPercentage { get; set; }
    }
}
