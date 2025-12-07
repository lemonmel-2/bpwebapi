namespace webapi.Model
{
    public class User
    {
        public string UserId { get; set; }

        public int HighestScore { get; set; }

        public User(string userId)
        {
            UserId = userId;
            HighestScore = 0;
        }
    }
}