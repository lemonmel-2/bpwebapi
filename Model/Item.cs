namespace webapi.Model
{
    public class Item
    {
        public string ItemID { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }

        public Item(string itemId, string name)
        {
            this.ItemID = itemId;
            this.Name = name;
        }
    }
}