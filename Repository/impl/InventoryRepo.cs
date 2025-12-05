using webapi.Data;
using webapi.Model;

namespace webapi.Repository.impl
{
    public class InventoryRepo : IInventoryRepo
    {
        private static GameContext _gameContext;

        public InventoryRepo(GameContext context)
        {
            _gameContext = context;
        }

        public async Task AddInventory(string userId, string itemId)
        {
            var currentInventory = _gameContext.Inventories.Find([userId, itemId]);
            if (currentInventory == null)
            {
                Inventory inventory = new Inventory(userId, itemId, 1);
                _gameContext.Inventories.Add(inventory);
            }
            else
            {
                currentInventory.Quantity += 1;
                _gameContext.Inventories.Update(currentInventory);
            }
            await _gameContext.SaveChangesAsync();
        }

        public List<Inventory> GetInventory(string userId)
        {
            return _gameContext.Inventories.Where(i => i.UserId == userId).ToList();
        }
    }
}