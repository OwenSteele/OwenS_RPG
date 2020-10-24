using System.IO;
using System.Xml;
using Engine.Models;
using Engine.Shared;

namespace Engine.Factories
{
    internal static class WorldFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Locations.xml";

        internal static World CreateWorld()
        {
            World world = new World();

            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath =
                    data.SelectSingleNode("/Locations")
                        .AttributeAsString("RootImagePath");

                LoadLocationsFromNodes(world,
                                       rootImagePath,
                                       data.SelectNodes("/Locations/Location"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }

            return world;
        }

        private static void LoadLocationsFromNodes(World world, string rootImagePath, XmlNodeList nodes)
        {
            if (nodes == null) return;

            foreach (XmlNode node in nodes)
            {
                Location location =
                    new Location(node.AttributeAsInt("X"),
                                 node.AttributeAsInt("Y"),
                                 node.AttributeAsString("Name"),
                                 node.SelectSingleNode("./Description")?.InnerText ?? "",
                                 $".{rootImagePath}{node.AttributeAsString("ImageFileName")}");

                AddMonsters(location, node.SelectNodes("./Monsters/Monster"));
                AddQuests(location, node.SelectNodes("./Quests/Quest"));
                AddTrader(location, node.SelectSingleNode("./Trader"));
                AddItem(location, node.SelectNodes("./Items/Item"));

                world.AddLocation(location);
            }
        }

        private static void AddMonsters(Location location, XmlNodeList monsters)
        {
            if (monsters == null) return;

            foreach (XmlNode monsterNode in monsters)
            {
                location.AddMonster(monsterNode.AttributeAsInt("ID"),
                                    monsterNode.AttributeAsInt("Percent"));
            }
        }

        private static void AddQuests(Location location, XmlNodeList quests)
        {
            if (quests == null) return;

            foreach (XmlNode questNode in quests)
            {
                location.QuestsAvailableHere
                        .Add(QuestFactory.GetQuestByID(questNode.AttributeAsInt("ID")));
            }
        }

        private static void AddTrader(Location location, XmlNode traderHere)
        {
            if (traderHere == null) return;

            location.TraderHere =
                TraderFactory.GetTraderByID(traderHere.AttributeAsInt("ID"));
        }
        private static void AddItem(Location location, XmlNodeList items)
        {
            if (items == null) return;

            foreach(XmlNode itemNode in items)
            {
                location.AddItemsToPickUp(itemNode.AttributeAsInt("ID"),
                                          itemNode.AttributeAsInt("Quantity"));
            }
        }
    }
}

#region old worldfactory code
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Engine.Models;

//namespace Engine.Factories
//{
//    internal static class WorldFactory //'class' = internal by default
//    {
//        internal static World CreateWorld()
//        {
//            World newWorld = new World();
//            #region Locations
//            newWorld.AddLocation(-2, -1, "Farmer's Field",
//                "There are rows of corn growing here, with giant rats hiding between them",
//                "FarmFields.png");
//            newWorld.AddLocation(-1, -1, "Farmer's House",
//                "This is the house of your neighbour, Farmer Ted.",
//                "Farmhouse.png");
//            newWorld.AddLocation(0, -1, "Home",
//                "This is your House.", 
//                "Home.png");
//            newWorld.AddLocation(-1, 0, "Trading Shop",
//                "The shop of Susan, the trader.",
//                "Trader.png");
//            newWorld.AddLocation(0, 0, "Town square",
//                "You see a fountain here.",
//                "TownSquare.png");
//            newWorld.AddLocation(1, 0, "Town Gate",
//                "There is a gate here, protecting the town from giant spiders.",
//                "TownGate.png");
//            newWorld.AddLocation(2, 0, "Spider Forest",
//                "The trees in this forest are covered with spider webs.",
//                "SpiderForest.png");
//            newWorld.AddLocation(0, 1, "Herbalist's hut",
//                "You see a small hut, with plants drying from the roof.",
//                "HerbalistsHut.png");
//            newWorld.AddLocation(0, 2, "Herbalist's garden",
//                "There are many plants here, with snakes hiding behind them.",
//                "HerbalistsGarden.png");
//            #endregion

//            #region Quests
//            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));
//            #endregion

//            #region Monsters
//            newWorld.LocationAt(0, 2).AddMonster(1, 100);
//            newWorld.LocationAt(-2, -1).AddMonster(2, 100);
//            newWorld.LocationAt(2, 0).AddMonster(3, 100);
//            #endregion

//            #region Traders
//            newWorld.LocationAt(-1, -1).TraderHere = TraderFactory.GetTraderByName("Farmer Ted");
//            newWorld.LocationAt(-1, 0).TraderHere = TraderFactory.GetTraderByName("Susan");
//            newWorld.LocationAt(0, 1).TraderHere = TraderFactory.GetTraderByName("Pete the Herbalist");
//            #endregion

//            #region Items To Pick Up
//            newWorld.LocationAt(0, -1).AddItemsToPickUp(3501, 1);
//            newWorld.LocationAt(0, -1).AddItemsToPickUp(3001, 1);
//            newWorld.LocationAt(0, -1).AddItemsToPickUp(3002, 1);
//            newWorld.LocationAt(0, -1).AddItemsToPickUp(3003, 1);
//            #endregion
//            return newWorld;
//        }
//        //internal static PlayerInteraction CreateInteractions()
//        //{
//        //    PlayerInteraction newInteractions = new PlayerInteraction();

//        //    newInteractions.AddAttackInteraction(0, false,
//        //        new [] { "you lose grip of your weapon and throw it a few feet behind you.",
//        //            "you had your eyes closed and miss completely.",
//        //            "the monster makes you jump and you dont have time to attack."
//        //        });

//        //    return newInteractions;
//        //}
//    }
//}
#endregion
