namespace webapi.Model
{
public class Item
    {
        public string ItemId { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }

        public Item(string itemId, string name)
        {
            ItemId = itemId;
            Name = name;
        }
    }
}