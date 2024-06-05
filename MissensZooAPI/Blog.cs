using MissensZooAPI.Controllers;

namespace MissensZooAPI
{
    public class Blog
    {
        public int blogId { get; set; }
        public string title { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string blogContent { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public string timestamp { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
    }
}
