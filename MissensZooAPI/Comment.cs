namespace MissensZooAPI
{
    public class Comment
    {
        public int blogId { get; set; }
        public int userId { get; set; }
        public string content { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
    }
}
