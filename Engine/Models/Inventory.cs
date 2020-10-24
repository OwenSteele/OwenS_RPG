using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Services;

namespace Engine.Models
{

    //uses functional style - never alters existing object properties, creates new instance with changed values
    public class Inventory
    {
        #region backing var
        private readonly List<GameItem> _backingInventory = new List<GameItem>();
        private readonly List<GroupedInventoryItem> _backingGroupedInventoryItems = new List<GroupedInventoryItem>();
        #endregion

        #region properties
        public IReadOnlyList<GameItem> Items => _backingInventory.AsReadOnly();
        public IReadOnlyList<GroupedInventoryItem> GroupedInventoryItems => _backingGroupedInventoryItems.AsReadOnly();
        public IReadOnlyList<GameItem> Weapons => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Weapon).AsReadOnly();
        //public IReadOnlyList<GameItem> Consumables => _backingInventory.ItemsThatAre(GameItem.ItemCategory.Consumable).AsReadOnly();
        public IReadOnlyList<GroupedInventoryItem> Consumables => _backingGroupedInventoryItems.FindAll(gi => gi.Item.Category == GameItem.ItemCategory.Consumable);

        public bool HasConsumable => Consumables.Any();
        #endregion

        #region constructors
        public Inventory(IEnumerable<GameItem> items = null)
        {
            if (items == null) return;

            foreach (GameItem item in items)
            {
                _backingInventory.Add(item);
                AddItemToGroupedInventory(item);
            }
        }
        #endregion

        #region functions
        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items) =>
            items.All(item => Items.Count(i => i.ItemTypeID == item.ItemID) >= item.Quantity);

        private void AddItemToGroupedInventory(GameItem item)
        {
            if (item.IsUnique)
                _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 1));
            else
            {
                if (_backingGroupedInventoryItems.All(gi => gi.Item.ItemTypeID != item.ItemTypeID))
                    _backingGroupedInventoryItems.Add(new GroupedInventoryItem(item, 0));

                _backingGroupedInventoryItems.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }
        }
        #endregion
    }
}
