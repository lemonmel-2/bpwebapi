using webapi.Model;

namespace webapi.Enum
{
    public class ItemsLibrary
    {
        public static readonly Item INVADER_001 = new Item("invader001", "purple invader");
        public static readonly Item INVADER_002 = new Item("invader002", "blue invader");
        public static readonly Item INVADER_003 = new Item("invader003", "pink invader");
        public static readonly Item FOOD_001 = new Item("food001", "burger");
        public static readonly Item FOOD_002 = new Item("food002", "carrot");

        private static readonly Dictionary<string, Item> _itemDict = new Dictionary<string, Item>
        {
            { INVADER_001.ItemID, INVADER_001 },
            { INVADER_002.ItemID, INVADER_002 },
            { INVADER_003.ItemID, INVADER_003 },
            { FOOD_001.ItemID, FOOD_001 },
            { FOOD_002.ItemID, FOOD_002 }
        };

        public static Dictionary<string, Item> GetAllItems()
        {
            return _itemDict;
        }

        public static Item GetItemById(string id)
        {
            return _itemDict[id];
        }

    }
}