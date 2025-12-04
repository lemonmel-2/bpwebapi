using webapi.Model;

namespace webapi.Repository
{
    public interface IUserRepo
    {
        public void AddUser(User user);

        public User GetUser(string userId);

        public void UpdateUser(User user);

        public User[] GetTopUsers();
    }
}