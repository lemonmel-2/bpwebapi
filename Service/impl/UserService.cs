using System.Security.Cryptography;
using webapi.Enum;
using webapi.Exception;
using webapi.Model;
using webapi.Repository;

namespace webapi.service.impl
{
    public class UserService : IUserService
    {
        private static IUserRepo _userRepo = UserRepo.Instance;

        private static string SALT_KEY = "salt";

        private static string PASSWORD = "password";
        
        public bool RecordNewScore(string userId, int score)
        {
            User user = _userRepo.GetUser(userId);
            if(score > user.HighestScore)
            {
                user.HighestScore = score;
                _userRepo.UpdateUser(user);
                return true;
            }
            return false;
        }

        public string Login(string userId, string pwd)
        {
            User user = _userRepo.GetUser(userId);
            if (user == null)
            {
                throw new GameException(ErrorCode.USER_NOT_EXIST);
            }
            else if(user.Pwd != Encrypt(pwd, user.Salt))
            {
                throw new GameException(ErrorCode.INCORRECT_PASSWORD);
            }

            return userId;
        }
        
        public User[] GetLeaderboard()
        {
            User[] users = _userRepo.GetTopUsers();
            return users;
        }

        public string Register(string userId, string pwd)
        {
            User existingUser = _userRepo.GetUser(userId);
            if (existingUser != null)
            {
                throw new GameException(ErrorCode.USER_ID_EXIST);
            }
            Dictionary<string, string> hashedPassword = Encrypt(pwd);
            User newUser = new User(userId, hashedPassword[PASSWORD], hashedPassword[SALT_KEY]);
            _userRepo.AddUser(newUser);
            return userId;
        }

        // Encrypt using into hashed string by random generated salt
        private Dictionary<string, string> Encrypt(string pwd)
        {
            Dictionary<string, string> hashDetails = new Dictionary<string, string>();
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var pbkdf2 = new Rfc2898DeriveBytes(pwd, salt, 1000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            hashDetails.Add(PASSWORD, Convert.ToBase64String(hash));
            hashDetails.Add(SALT_KEY, Convert.ToBase64String(salt));
            return hashDetails;
        }

        // Encrypt paswword into hashed string by user database salt
        private string Encrypt(string pwd, string salt)
        {
            byte[] pwdByte = System.Text.Encoding.UTF8.GetBytes(pwd);
            byte[] saltByte = Convert.FromBase64String(salt);
            using var pbkdf2 = new Rfc2898DeriveBytes(pwdByte, saltByte, 1000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);
            return Convert.ToBase64String(hash);
        }
    }
}