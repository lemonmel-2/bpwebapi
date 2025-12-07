namespace webapi.Model
{
    public class UserCredential
    {
        public string UserId { get; set; }

        public string Pwd { get; set; }

        public string Salt { get; set; }

        public UserCredential(string userId, string pwd, string salt)
        {
            UserId = userId;
            Pwd = pwd;
            Salt = salt;
        }
    }
}