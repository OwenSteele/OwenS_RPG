using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EventArgs
{
    public class GameMessageEventArgs : System.EventArgs
    {
        public string Message { get; private set; }

        public enum ColorCategory
        {
            None,
            Player,
            Quest,
            BattlePlayer,
            BattleOpponent,
            Battle,
            Actions
        }
        public string Color { get; private set; }

        public static Dictionary<ColorCategory, string> ColorCodes = new Dictionary<ColorCategory, string>();
        
        public void InitializeGameMessageEventArgs()
        {
            ColorCodes.Add(ColorCategory.None, "#000000");
            ColorCodes.Add(ColorCategory.Quest, "#8B008B");
            ColorCodes.Add(ColorCategory.Battle, "#191970");
            ColorCodes.Add(ColorCategory.Actions, "#8B0000");
            ColorCodes.Add(ColorCategory.BattlePlayer, "#006400");
            ColorCodes.Add(ColorCategory.BattleOpponent, "#4B0082");
        }

        public GameMessageEventArgs(string message, ColorCategory color = ColorCategory.None)
        {
            if (!ColorCodes.Any()) InitializeGameMessageEventArgs();

            Message = message;
            Color = ColorCodes.FirstOrDefault(c => c.Key == color).Value;
        }

    }

}
