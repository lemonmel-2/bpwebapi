using webapi.Model;

namespace webapi.service
{
    public interface IUserService
    {
        public string Login(string userId, string pwd);
        
        public string Register(string userId, string pwd);

        public Task<bool> RecordNewScore(string userId, int score);

        public User GetUser(string userId);

        public List<User> GetLeaderboard();
    }
}