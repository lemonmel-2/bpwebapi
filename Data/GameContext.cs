using Microsoft.EntityFrameworkCore;
using webapi.Model;

namespace webapi.Data
{
    public class GameContext: DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        
        public DbSet<UserCredential> UserCredentials {get; set;} 
        public DbSet<Inventory> Inventories {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Inventory>().HasKey(ui => new {ui.UserId, ui.ItemId});
            modelBuilder.Entity<UserCredential>().HasKey(uc => uc.UserId);
        }

    }
}