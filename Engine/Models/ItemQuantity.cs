using Engine.Factories;

namespace Engine.Models
{
    public class ItemQuantity
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }

        public string Description => 
            $"{Quantity}x {ItemFactory.ItemName(ItemID)}";

        public ItemQuantity(int itemID, int quantity)
        {
            ItemID = itemID;
            Quantity = quantity;
        }

    }
}
