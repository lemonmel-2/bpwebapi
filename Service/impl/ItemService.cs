using webapi.Enum;
using webapi.Exception;
using webapi.Model;
using webapi.Repository;

namespace webapi.service.impl
{
    public class ItemService : IItemService
    {
        private static IUserRepo _userRepo = UserRepo.Instance;

        private static Random rand = new Random();

        public void AddItem(string userId, string itemId)
        {
            try
            {
                 User user = _userRepo.GetUser(userId);
                Dictionary<string, Item> userItems = user.Items;
                if(userItems.ContainsKey(itemId))
                {
                    user.Items[itemId].Quantity += 1;
                }
                else
                {

                    Item item = ItemsLibrary.GetItemById(itemId);
                    user.Items.Add(itemId, item);
                }
                _userRepo.UpdateUser(user);
            }
            catch(KeyNotFoundException)
            {
                throw new GameException(ErrorCode.INVALID_ITEM);
            }
        }

        public Item GenerateItem(string userId)
        {
            var keyList = new List<string>(ItemsLibrary.GetAllItems().Keys);
            string randomKey = keyList[rand.Next(keyList.Count)];
            return ItemsLibrary.GetItemById(randomKey);
        }

        public Item[] GetItems(string userId)
        {
            User user = _userRepo.GetUser(userId);
            return user.Items.Values.ToArray();
        }
    }
}