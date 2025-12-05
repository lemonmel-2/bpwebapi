namespace webapi.Model
{
    public class User
    {
        public string UserId { get; set; }

        public string Pwd { get; set; }

        public string Salt { get; set; }

        public int HighestScore { get; set; }

        public User(string userId, string pwd, string salt)
        {
            UserId = userId;
            Pwd = pwd;
            Salt = salt;
            HighestScore = 0;
        }
    }
}