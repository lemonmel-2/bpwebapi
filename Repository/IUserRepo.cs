using webapi.Model;

namespace webapi.Repository
{
    public interface IUserRepo
    {
        public Task AddUser(User user);

        public User GetUser(string userId);

        public Task UpdateUser(User user);

        public List<User> GetTopUsers();
    }
}