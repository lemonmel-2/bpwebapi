namespace webapi.Model
{
    public class Token
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public int ExpireIn { get; set; }

        public Token(string token, string tokenType, int expireIn)
        {
            AccessToken = token;
            TokenType = tokenType;
            ExpireIn = expireIn;
        }
    }
}