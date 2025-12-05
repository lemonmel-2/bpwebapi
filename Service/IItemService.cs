using webapi.Model;

namespace webapi.service
{
    public interface IItemService
    {
        public List<Item> GetItems(string userId);

        public Item GenerateItem();

        public void AddItem(string userId, string itemId);

    }
}