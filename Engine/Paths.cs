using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Engine
{
    public class Paths
    {
        private static string MainPath { get => "/Engine;component"; }
        public static class Images
        {
            private static readonly string imagesPath = MainPath + "/Images";
            private static readonly string locations = "/Locations";
            private static readonly string monsters = "/Monsters";
            private static readonly string buttons = "/Buttons";
            private static readonly string uiBackgrounds = "/UIBackground";
            private static readonly string keyboardKeys = "/keyboardKeys";

            public static string Locations(string file) => imagesPath + locations + "/" + file;
            public static string Monsters(string file) => imagesPath + monsters + "/" + file;
            public static string Buttons(string file) => imagesPath + buttons + "/" + file;
            public static string UIBackgrounds(string file) => imagesPath + uiBackgrounds + "/" + file;
            public static string KeyboardKeys(string file) => imagesPath + keyboardKeys + "/" + file;
        }
    }
}
