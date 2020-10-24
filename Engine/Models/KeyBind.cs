using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using System.Windows;

namespace Engine.Models
{
    public class KeyBind
    {
        public string ID { get; set; }
        public string KeyAction { get; set; }

        public KeyBind(string id, string keyAction)
        {
            ID = id;
            KeyAction = keyAction;
        }
    }

}
