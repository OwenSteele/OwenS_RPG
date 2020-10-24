using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class ImageFactory
    {
        public static class Button
        {
            public static string North { get; } = Paths.Images.Buttons("North.png");
            public static string West { get; } = Paths.Images.Buttons("West.png");
            public static string East { get; } = Paths.Images.Buttons("East.png");
            public static string South { get; } = Paths.Images.Buttons("West - Copy.png"); //for some reason South is not accepted as a name...
            public static string Trader { get; } = Paths.Images.Buttons("Trader.png");
            public static string Map { get; } = Paths.Images.Buttons("Map.png");
            public static string Attack { get; } = Paths.Images.Buttons("AttackRS.png");
            public static string UseConsumable { get; } = Paths.Images.Buttons("Consumable.png");
            public static string UIInventoryTabQuests { get; } = Paths.Images.Buttons("InventoryTabQuestsRS.png");
            public static string UIInventoryTabBackpack { get; } = Paths.Images.Buttons("InventoryTabBackpackRS.png");
            public static string UIInventoryTabRecipe { get; } = Paths.Images.Buttons("InventoryTabRecipeRS.png");
            public static string InventoryTabRecipeCraft { get; } = Paths.Images.Buttons("InventoryTabRecipeCraftRS.png");

        }

        public static class UIBackground
        {
            //main window only
            public static string PlayerStatsCenter { get; } = Paths.Images.UIBackgrounds("PlayerStatsCenterLeather2.png");
            public static string PlayerStatsEdge { get; } = Paths.Images.UIBackgrounds("PlayerStatsEdge.png");
            public static string PlayerStatsGoldIcon { get; } = Paths.Images.UIBackgrounds("PlayerStatsGoldIcon.png");
            public static string GameMessagesCenter { get; } = Paths.Images.UIBackgrounds("GameMessagesCenter.png");
            public static string LocationBackgroundCenter { get; } = Paths.Images.UIBackgrounds("LocationBackgroundCenter.png");
            public static string MonsterBackgroundNameBanner { get; } = Paths.Images.UIBackgrounds("MonsterBackgroundNameBanner.png");
            public static string ActionPanelBackgroundCenter { get; } = Paths.Images.UIBackgrounds("ActionBackgroundCenter.png");
            public static string MonsterBackgroundCenterDefault { get; } = Paths.Images.UIBackgrounds("MonsterBackGroundCenterNEW.png");

            public class Dynamic
            {
                public static string MonsterBackgroundCenterHome { get; } = Paths.Images.UIBackgrounds("homelocationidea.png");
            }

            public static class Map
            {
                //map window only
                public static string MainOverallBackground { get; } = Paths.Images.UIBackgrounds("MapScreenBackgroundBlank.png");
                public static string MapScreenCompass { get; } = Paths.Images.UIBackgrounds("MapScreenCompass.png");
            }

            public static class Trader
            {
                //trader window only
                public static string MainOverallBackground { get; } = Paths.Images.UIBackgrounds("TraderScreen_MainOverallBackgroundWood.png");
            }

            public static class UserDetails
            {
                public static string MainOverallBackground { get; } = Paths.Images.UIBackgrounds("HomeLocationIdea.png");
            }
        }

        public static class KeyboardKeys
        {
            public static string BlackBlankCenter { get; } = Paths.Images.KeyboardKeys("KeyBoardKeyBlankCenter.png");
            public static string BlackBlankLeft { get; } = Paths.Images.KeyboardKeys("KeyBoardKeyBlankLeft.png");
            public static string BlackBlankRight { get; } = Paths.Images.KeyboardKeys("KeyBoardKeyBlankRight.png");
        }
    }
}
