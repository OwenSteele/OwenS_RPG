using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class CharacterClass
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int Dexterity { get; set; }

        public CharacterClass(int id, string name, int dexterity)
        {
            ID = id;
            Name = name;
            Dexterity = DexterityBounds(dexterity);
        }
        private int DexterityBounds(int dexterity)
        {
            if (dexterity > 18) return 18;
            if (dexterity < 3) return 3;
            return dexterity;
        }
    }
}
