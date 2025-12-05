using Microsoft.EntityFrameworkCore;
using webapi.Model;

namespace webapi.Data
{
    public class GameContext: DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // This becomes the Users table

        public DbSet<Inventory> Inventories {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Inventory>().HasKey(ui => new {ui.UserId, ui.ItemId});
        }

    }
}