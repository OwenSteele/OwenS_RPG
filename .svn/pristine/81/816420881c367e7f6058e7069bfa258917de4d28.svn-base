using Engine.Models;
using System;

namespace Engine.Actions
{
    public class Heal : BaseAction, IAction
    {
        private readonly int _hitPointsToHeal;

        public Heal(GameItem itemInUse, int hitPointsToHeal) : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Consumable)
                throw new ArgumentException($"{itemInUse.Name} is not consumable.");

            _hitPointsToHeal = hitPointsToHeal;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "yourself" : $"the {target.Name.ToLower()}";

            ReportResult($"{actorName} healed {targetName} for {_hitPointsToHeal} health points.");
            target.Heal(_hitPointsToHeal);
        }
    }
}
