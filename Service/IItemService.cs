using webapi.Model;

namespace webapi.service
{
    public interface IItemService
    {
        public Item[] GetItems(string userId);

        public Item GenerateItem(string userId);

        public void AddItem(string userId, string itemId);

    }
}