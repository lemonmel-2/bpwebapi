using System.Text.Json.Serialization;

namespace webapi.Model
{
    public class User
    {
        public string UserId { get; set; }

        public string Pwd { get; set; }

        public string Salt { get; set; }

        public int HighestScore { get; set; }

        public Dictionary<string, Item> Items { get; set; }
        public User(string userId, string pwd, string salt)
        {
            this.UserId = userId;
            this.Pwd = pwd;
            this.Salt = salt;
            this.HighestScore = 0;
            this.Items = new Dictionary<string, Item>();
        }


        [JsonConstructor]
        public User(string userId, string pwd, string salt, int highestScore, Dictionary<string, Item> items)
        {
            this.UserId = userId;
            this.Salt = salt;
            this.Pwd = pwd;
            this.HighestScore = highestScore;
            this.Items = items;
        }
    }
}