using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class MonsterEncounter
    {
        public int MonsterID { get; }
        public int ChanceOfEncountering { get; set; }

        public MonsterEncounter(int monsterId, int chanceOfEncountering)
        {
            MonsterID = monsterId;
            ChanceOfEncountering = chanceOfEncountering;
        }
    }
}
