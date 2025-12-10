using System.Security.Cryptography;
using webapi.Enum;
using webapi.Exception;
using webapi.Model;
using webapi.Repository;

namespace webapi.service.impl
{
    public class UserService : IUserService
    {
        private static IUserRepo _userRepo;

        private static string SALT_KEY = "salt";

        private static string PASSWORD = "password";

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        
        public User GetUser(string userId)
        {
            User user = _userRepo.GetUser(userId);
            if (user == null)
            {
                throw new GameException(ErrorCode.USER_NOT_EXIST);
            }
            return user;
        }

        public UserCredential GetUserCredential(string userId)
        {
            UserCredential userCredential = _userRepo.GetUserCredential(userId);
            if (userCredential == null)
            {
                throw new GameException(ErrorCode.USER_NOT_EXIST);
            }
            return userCredential;
        }

        public async Task<bool> RecordNewScore(string userId, int score)
        {
            User user = _userRepo.GetUser(userId);
            bool breakRecord = false;
            if(score > user.HighestScore)
            {
                user.HighestScore = score;
                breakRecord = true;
            }
            user.Point += score ;
            await _userRepo.UpdateUser(user);
            return breakRecord;
        }

        public string Login(string userId, string pwd)
        {
            UserCredential userCredential = GetUserCredential(userId);
            if(userCredential.Pwd != Encrypt(pwd, userCredential.Salt))
            {
                throw new GameException(ErrorCode.INCORRECT_PASSWORD);
            }
            return userId;
        }
        
        public List<User> GetLeaderboard()
        {
            List<User> users = _userRepo.GetTopUsers();
            return users;
        }

        public async Task UpdateUserPoints(string userId, int pointToDeduct)
        {
            User user = _userRepo.GetUser(userId);
            if(user.Point < pointToDeduct)
            {
                throw new GameException(ErrorCode.INSUFFICIENT_POINTS);
            }
            user.Point -= pointToDeduct;
            await _userRepo.UpdateUser(user);
        }

        public string Register(string userId, string pwd)
        {
            User existingUser = _userRepo.GetUser(userId);
            if (existingUser != null)
            {
                throw new GameException(ErrorCode.USER_ID_EXIST);
            }
            Dictionary<string, string> hashedPassword = Encrypt(pwd);
            UserCredential newUserCredential = new UserCredential(userId, hashedPassword[PASSWORD], hashedPassword[SALT_KEY]);
            _userRepo.AddUser(newUserCredential);
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