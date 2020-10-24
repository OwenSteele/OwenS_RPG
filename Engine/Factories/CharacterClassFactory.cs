using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class CharacterClassFactory
    {
        private static readonly List<CharacterClass> _characterClasses = new List<CharacterClass>();
        public static List<CharacterClass> CreateCharacterClasses()
        {

            _characterClasses.Add(new CharacterClass(0, "Fighter", 5));
            _characterClasses.Add(new CharacterClass(1, "Medic",4));
            _characterClasses.Add(new CharacterClass(2, "Pacifist",3));
            _characterClasses.Add(new CharacterClass(3, "Monster Whisperer",4));

            return _characterClasses;
        }
        public static CharacterClass GetCharacterClassByName(string characterclass) =>
            _characterClasses.FirstOrDefault(c => c.Name.ToLower() == characterclass.ToLower());
        public static CharacterClass GetCharacterClassByID(int characterclass) =>
            _characterClasses.FirstOrDefault(c => c.ID == characterclass);

    }
}
