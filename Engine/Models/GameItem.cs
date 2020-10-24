using Engine.Actions;
using System;

namespace Engine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable,
            Recipe
        }

        public ItemCategory Category { get; }
        public int ItemTypeID { get; }
        public string Name { get; }
        public int Price { get; }
        public bool IsUnique { get; }
        public int MinimumDamage { get; }
        public int MaximumDamage { get; }
        public IAction Action { get; set; }
        public int RecipeID { get; set; }


        public GameItem (ItemCategory category, int itemTypeID, string name, int price, 
            bool isUnique = false, IAction action = null, int recipeID = 0)
        {
            Category = category;
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Action = action;
            RecipeID = recipeID;
        }
        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeID, Name, Price, IsUnique, Action, RecipeID);
        }

        public void PerformAction(LivingEntity actor, LivingEntity target) =>
            Action?.Execute(actor, target);
    }
}
