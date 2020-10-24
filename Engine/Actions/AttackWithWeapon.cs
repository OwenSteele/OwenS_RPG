using System;
using Engine.Models;
using Engine.Services;

namespace Engine.Actions
{
    public class AttackWithWeapon : BaseAction, IAction
    {
        private readonly int _minimumDamage;
        private readonly int _maximumDamage;

        public AttackWithWeapon(GameItem itemInUse, int minimumDamage, int maximumDamage) : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
                throw new ArgumentException($"{itemInUse.Name} is not a weapon.");
            if (minimumDamage < 0)
                throw new ArgumentException("ERROR: min. damage cannot be less than 0.");
            if (maximumDamage < minimumDamage)
                throw new ArgumentException("Error: max. damage must be more than or equal to min. damage.");

            _minimumDamage = minimumDamage;
            _maximumDamage = maximumDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
            string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";

            if (CombatService.AttackSucceeded(actor,target))
            {
                int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);
                ReportResult($"{actorName} did {damage} point{((damage > 1) ? "s" : "")} of damage to {targetName}.");

                target.TakeDamage(damage);
            }
            else 
                ReportResult($"{actorName} missed {targetName}.");
        }
    }
}
