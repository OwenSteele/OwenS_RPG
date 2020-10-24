using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public static class UserDetailsMessage
    {
        private static string _nameCheckMessage;

        private static readonly string[] curses = { "shit", "fuck", "crap" };
        private static readonly string[] insults = { "bitch", "dick", "twat", "wanker", "bastard", "cunt", "pussy", "slut" };
        private static readonly string[] immaturity = { "penis", "willy", "bum", "fanny", "boob", "tit", "sex" };

        public static bool LeftHandedControls { get; set; }
        public static bool OpenTutorialWindowOnLaunch { get; set; }
        public static string NameCheckMessage
        {
            get => _nameCheckMessage; 
            private set { _nameCheckMessage = value; }
        }
        public static string CheckNameIsValid(string name)
        {
            string result = null;

                if (name.Length > 0)
                {
                    switch (name.ToLower())
                    {
                        case string lengthOfValue when lengthOfValue.Length < 3: result = "That name is too short!"; break;
                        case string lengthOfValue when lengthOfValue.Length > 20: result = "That name is too long!"; break;
                        case string curse when curses.Any(curse.Contains): result = "Ooh you're hard. (Valid)"; break;
                        case string insult when insults.Any(insult.Contains): result = "That's just rude. (Valid)"; break;
                        case string immature when immaturity.Any(immature.Contains): result = "Aren't you mature. (Valid)"; break;
                        default: result = "Valid."; break;
                    }
                }
                else result = "You need to enter a name!";

                _nameCheckMessage = result;

            return result;
        }
    }
}
