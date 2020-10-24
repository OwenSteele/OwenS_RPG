using Engine.Models;
using Engine.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestEngine.Services
{
    [TestClass]
    public class TestCombatService
    {
        [TestMethod]
        public void Test_FirstAttacker()
        {
            // Player and monster with dexterity 12
            Player player = new Player("",18, 0, 0, 0, 0,  0);
            Monster monster = new Monster(0, "",12, "", 0,  null, 0, 0);

            CombatService.Combatant result = CombatService.FirstAttacker(player, monster);
        }
    }
}
