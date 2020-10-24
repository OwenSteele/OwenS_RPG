using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Engine.Models;
using Engine.Shared;
using System;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";

        private static readonly List<Monster> _baseMonsters = new List<Monster>();

        static MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/Monsters")
                        .AttributeAsString("RootImagePath");

                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Monster"), rootImagePath);
            }
            else
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
        }

        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if (nodes == null) return;

            foreach (XmlNode node in nodes)
            {
                Monster monster =
                    new Monster(node.AttributeAsInt("ID"),
                                node.AttributeAsString("Name"),
                                Convert.ToInt32(node.SelectSingleNode("./Dexterity").InnerText),
                                $".{rootImagePath}{node.AttributeAsString("ImageFileName")}",
                                node.AttributeAsInt("MaximumHitPoints"),
                                ItemFactory.CreateGameItem(node.AttributeAsInt("WeaponID")),
                                node.AttributeAsInt("RewardXP"),
                                node.AttributeAsInt("Gold"));

                XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");
                if (lootItemNodes != null)
                {
                    foreach (XmlNode lootItemNode in lootItemNodes)
                    {
                        monster.AddItemToLootTable(lootItemNode.AttributeAsInt("ID"),
                                                   lootItemNode.AttributeAsInt("Percentage"));
                    }
                }

                _baseMonsters.Add(monster);
            }
        }

        public static Monster GetMonster(int id) => _baseMonsters.FirstOrDefault(m => m.ID == id)?.GetNewInstance();
    }
}

#region old monsterfactory code
//using Engine.Models;
//using System;

//namespace Engine.Factories
//{
//    public static class MonsterFactory
//    {
//        public static Monster GetMonster(int monsterID)
//        {
//            switch (monsterID)
//            {
//                case 1:
//                    Monster snake = new Monster(1,"Snake", 4, ItemFactory.CreateGameItem(1501), 5, 1);

//                    snake.CurrentWeapon = ItemFactory.CreateGameItem(1501);

//                    AddLootItem(snake,9001,25);
//                    AddLootItem(snake, 9002, 75);

//                    return snake;
//                case 2:
//                    Monster rat = new Monster(2, "Rat", 5,ItemFactory.CreateGameItem(1502), 5, 1);

//                    rat.CurrentWeapon = ItemFactory.CreateGameItem(1502);

//                    AddLootItem(rat,9003,25);
//                    AddLootItem(rat, 9004, 75);

//                    return rat;
//                case 3:
//                    Monster giantSpider = new Monster(3, "Giant Spider",  10, ItemFactory.CreateGameItem(1503),10,3);

//                    giantSpider.CurrentWeapon = ItemFactory.CreateGameItem(1503);

//                    AddLootItem(giantSpider, 9005, 25);
//                    AddLootItem(giantSpider, 9006, 75);

//                    return giantSpider;
//                default:
//                    throw new ArgumentException(string.Format(
//                        $"MonsterType \"{monsterID}\" does not exist in the current context."));
//            }
//        }
//        private static void AddLootItem(Monster monster, int itemID, int percentage)
//        {
//            if (RandomNumberGenerator.NumberBetween(1, 100) <= percentage)
//                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemID));
//        }
//    }
//}
#endregion
