using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Enum;
using webapi.Exception;
using webapi.Model;

namespace webapi.Repository.impl
{
    public class UserRepo : IUserRepo
    {
        private static GameContext _gameContext;

        public UserRepo(GameContext context)
        {
            _gameContext = context;
        }

        public async Task AddUser(User user)
        {
            try
            {
                _gameContext.Users.Add(user);
                await _gameContext.SaveChangesAsync();
            }
            catch (System.Exception e)
            {
                throw new GameException(ErrorCode.DATABASE_ERROR);
            }
        }

        public List<User> GetTopUsers()
        {
            return  _gameContext.Users
                .OrderByDescending(u => u.HighestScore)
                .Take(5)
                .ToList();
        }

        public User GetUser(string userId)
        {
            try
            {
                var user = _gameContext.Users.Find(userId);
                return user;
            }
            catch (System.Exception e)
            {
                throw new GameException(ErrorCode.DATABASE_ERROR);
            }
        }

        public async Task UpdateUser(User user)
        {
            try
            {
                var userToUpdate = await _gameContext.Users.FirstOrDefaultAsync(i => i.UserId == user.UserId);
                if (userToUpdate == null)
                {
                    throw new GameException(ErrorCode.USER_NOT_EXIST);
                }
                _gameContext.Users.Update(user);
                await _gameContext.SaveChangesAsync();
            }
            catch (GameException)
            {
                throw;
            }
            catch (System.Exception e)
            {
                throw new GameException(ErrorCode.DATABASE_ERROR);
            }
        }
    }
}