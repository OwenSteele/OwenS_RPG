using System.Dynamic;

namespace Engine.Models
{
    public class Trader : LivingEntity
    {
        public int ID { get; }
        public double ValueAdjust { get; set; }

        public Trader(int id, string name, double valueAdjust) : base(name, 18, 9999, 9999, 9999)
        {
            ID = id;
            ValueAdjust = valueAdjust;
        }
    }
}
