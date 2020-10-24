using Engine.Models;
using System.Collections.Generic;

namespace Engine.Factories
{
    public static class KeyBindingFactory
    {
        public static List<KeyBind> CreateUserInputKeys(bool leftHandedControls)
        {
            if (leftHandedControls)
            {
                //must be in this order
                KeyBindingSet leftKeyBindings = new KeyBindingSet();
                leftKeyBindings.AddKey("I", "North");
                leftKeyBindings.AddKey("J", "West");
                leftKeyBindings.AddKey("K", "South");
                leftKeyBindings.AddKey("L", "East");

                leftKeyBindings.AddKey("Space", "Attack");
                leftKeyBindings.AddKey("H", "Heal");
                leftKeyBindings.AddKey("T", "Trader");
                leftKeyBindings.AddKey("M", "Map");
                leftKeyBindings.AddKey("Escape", "Tutorial");
            }
            else
            {
                KeyBindingSet rightKeyBindings = new KeyBindingSet();
                rightKeyBindings.AddKey("W", "North");
                rightKeyBindings.AddKey("A", "West");
                rightKeyBindings.AddKey("S", "South");
                rightKeyBindings.AddKey("D", "East");

                rightKeyBindings.AddKey("Space", "Attack");
                rightKeyBindings.AddKey("H", "Heal");
                rightKeyBindings.AddKey("T", "Trader");
                rightKeyBindings.AddKey("M", "Map");
                rightKeyBindings.AddKey("Escape", "Tutorial");
            }
            return KeyBindingSet.GetCurrentKeyBindingSet();
        }
    }
}
