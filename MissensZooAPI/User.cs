using MissensZooAPI.Controllers;


namespace MissensZooAPI
{
    public class User
    {
        public int id { get; set; }
        public int role { get; set; } = 1;
        public string userName { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string eMail { get; set; } = string.Empty;

    }
}
