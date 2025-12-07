using webapi.Model;

namespace webapi.Repository
{
    public interface IUserRepo
    {
        public Task AddUser(UserCredential userCredential);

        public User GetUser(string userId);

        public UserCredential GetUserCredential(string userId);

        public Task UpdateUser(User user);

        public List<User> GetTopUsers();
    }
}