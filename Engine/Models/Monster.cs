using System.Collections.Generic;
using Engine.Factories;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable =
            new List<ItemPercentage>();

        public int ID { get; }
        public string ImageFileName { get; }
        public int RewardExperiencePoints { get; }

        public Monster(int id, string name,
                       int dexterity, string imageFileName,
                       int maximumHitPoints,
                       GameItem currentWeapon,
                       int rewardExperiencePoints, int gold) :
            base(name, dexterity, maximumHitPoints, maximumHitPoints, gold)
        {
            ID = id;
            ImageFileName = imageFileName;
            CurrentWeapon = currentWeapon;
            RewardExperiencePoints = rewardExperiencePoints;
        }

        public void AddItemToLootTable(int id, int percentage)
        {
            // Remove the entry from the loot table,
            // if it already contains an entry with this ID
            _lootTable.RemoveAll(ip => ip.ID == id);

            _lootTable.Add(new ItemPercentage(id, percentage));
        }

        public Monster GetNewInstance()
        {
            Monster newMonster =
                new Monster(ID, Name, Dexterity, ImageFileName, MaximumHitPoints, CurrentWeapon,
                            RewardExperiencePoints, Gold);

            foreach (ItemPercentage itemPercentage in _lootTable)
            {
                // Clone the loot table
                newMonster.AddItemToLootTable(itemPercentage.ID, itemPercentage.Percentage);

                // Populate the new monster's inventory, using the loot table
                if (RandomNumberGenerator.NumberBetween(1, 100) <= itemPercentage.Percentage)
                {
                    newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.ID));
                }
            }

            return newMonster;
        }
    }
}

//
