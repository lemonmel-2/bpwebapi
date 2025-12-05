namespace webapi.Model
{
    public class Inventory
    {
        public string UserId { get; set; }

        public string ItemId { get; set; }

        public int Quantity { get; set; }

        public Inventory(string userId, string itemId, int quantity)
        {
            UserId = userId;
            ItemId = itemId;
            Quantity = quantity;
        }
    }
}