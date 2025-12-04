using Microsoft.EntityFrameworkCore;
using webapi.Model;

namespace webapi.Data
{
    public class GameContext: DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // This becomes the Users table

        public DbSet<Item> Items { get; set; }

        public DbSet<UserItem> UserItems {get; set;}

    }
}