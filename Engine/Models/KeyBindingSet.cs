using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class KeyBindingSet
    {
        private static List<KeyBind> _keyBindings = new List<KeyBind>();

        internal void AddKey(string keyID, string keyAction)=>
            _keyBindings.Add(new KeyBind(keyID, keyAction));

        public static List<KeyBind> GetCurrentKeyBindingSet() => _keyBindings;
    }
}
