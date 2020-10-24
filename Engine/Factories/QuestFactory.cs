using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Engine.Models;
using Engine.Shared;

namespace Engine.Factories
{
    internal static class QuestFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Quests.xml";

        private static readonly List<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                LoadQuestsFromNodes(data.SelectNodes("/Quests/Quest"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        private static void LoadQuestsFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
                List<ItemQuantity> rewardItems = new List<ItemQuantity>();
                List<ItemQuantity> prerequisiteItems = new List<ItemQuantity>();
                List<int> prerequisiteQuests = new List<int>();
                int prerequisiteLevel = 0;

                if (node.SelectSingleNode("./ ItemsToComplete") != null)
                    foreach (XmlNode childNode in node.SelectNodes("./ItemsToComplete/Item"))
                    {
                    itemsToComplete.Add(new ItemQuantity(childNode.AttributeAsInt("ID"),
                                                         childNode.AttributeAsInt("Quantity")));
                    }

                if (node.SelectSingleNode("./RewardItems") != null)
                    foreach (XmlNode childNode in node.SelectNodes("./RewardItems/Item"))
                    {
                    rewardItems.Add(new ItemQuantity(childNode.AttributeAsInt("ID"),
                                                     childNode.AttributeAsInt("Quantity")));
                    }

                if (node.SelectSingleNode("./Prerequisites") != null)
                {
                    if (node.SelectSingleNode("./Prerequisites/Items") != null)
                        foreach (XmlNode childNode in node.SelectNodes("./Prerequisites/Items/Item"))
                        {
                            prerequisiteItems.Add(new ItemQuantity(childNode.AttributeAsInt("ID"),
                                                                   childNode.AttributeAsInt("Quantity")));
                        }

                    if (node.SelectSingleNode("./Prerequisites/Quests") != null)
                        foreach (XmlNode childNode in node.SelectNodes("./Prerequisites/Quests/Quest"))
                            prerequisiteQuests.Add(childNode.AttributeAsInt("ID"));
                    //the quest objects themselves cannot be added, as they may not have been instantiated from the xml yet.

                    if (node.SelectSingleNode("./Prerequisites").Attributes?["Level"] != null)
                        prerequisiteLevel = node.SelectSingleNode("./Prerequisites").AttributeAsInt("Level");
                }

                _quests.Add(new Quest(node.AttributeAsInt("ID"),
                                      node.SelectSingleNode("./Name")?.InnerText ?? "",
                                      node.SelectSingleNode("./Description")?.InnerText ?? "",
                                      itemsToComplete,
                                      node.AttributeAsInt("RewardExperiencePoints"),
                                      node.AttributeAsInt("RewardGold"),
                                      rewardItems,
                                      prerequisiteLevel,
                                      prerequisiteItems,
                                      prerequisiteQuests));
            }
        }

        internal static Quest GetQuestByID(int id)
        {
            return _quests.FirstOrDefault(quest => quest.ID == id);
        }
    }
}
