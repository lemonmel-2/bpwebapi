using webapi.Enum;
using webapi.Exception;
using webapi.Model;
using webapi.Repository;

namespace webapi.service.impl
{
    public class ItemService : IItemService
    {
        private static IInventoryRepo _inventoryRepo;

        private static Random rand = new Random();

        public ItemService(IInventoryRepo inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        public void AddItem(string userId, string itemId)
        {
            try
            {
                Item item = ItemsLibrary.GetItemById(itemId);
                _inventoryRepo.AddInventory(userId, itemId);
            }
            catch (KeyNotFoundException)
            {
                throw new GameException(ErrorCode.INVALID_ITEM);
            }
        }

        public Item GenerateItem()
        {
            var keyList = new List<string>(ItemsLibrary.GetAllItems().Keys);
            string randomKey = keyList[rand.Next(keyList.Count)];
            return ItemsLibrary.GetItemById(randomKey);
        }

        public List<Item> GetItems(string userId)
        {
            List<Item> items = new List<Item>();
            List<Inventory> inventory = _inventoryRepo.GetInventory(userId);
            foreach(Inventory i in inventory)
            {
                Item item = ItemsLibrary.GetItemById(i.ItemId);
                item.Quantity = i.Quantity;
                items.Add(item);
            }
            return items;
        }
    }
}