using webapi.Model;

namespace webapi.Repository
{
    public interface IInventoryRepo
    {
        public Task AddInventory(string userId, string itemId);

        public List<Inventory> GetInventory(string userId);

    }
}