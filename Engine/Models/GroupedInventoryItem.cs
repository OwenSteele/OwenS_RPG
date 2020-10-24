using System;

namespace Engine.Models  
{
    public class GroupedInventoryItem : BaseNotificationClass
    {
        private GameItem _item;
        private int _quantity;

        public GameItem Item
        {
            get => _item; 
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }
        public int Quantity
        {
            get => _quantity; 
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public string ToolTipContents =>
           Item.Category.ToString();

        public GroupedInventoryItem(GameItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }
}
