using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.EventArgs;
using Engine.Services;

namespace Engine.Models
{
    public class Battle : IDisposable
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Player _player;
        private readonly Monster _opponent;

        public event EventHandler<CombatVictoryEventArgs> OnCombatVictory;

        public Battle(Player player, Monster opponent)
        {
            _player = player;
            _opponent = opponent;

            _player.OnActionPerformed += OnCombatantActionPerformed;
            _opponent.OnActionPerformed+= OnCombatantActionPerformed;
            _opponent.OnKilled += OnOpponentKilled;

            _messageBroker.RaiseMessage($"\nYou have encountered {StringGrammar.BeginsWithVowel(opponent.Name, true)}!", color: GameMessageEventArgs.ColorCategory.Battle);

            if (CombatService.FirstAttacker(_player,_opponent) == CombatService.Combatant.Opponent) 
                AttackPlayer();
        }

        public void AttackOpponent()
        {
            if (_player.CurrentWeapon == null)
            {
                _messageBroker.RaiseMessage("You need a weapon to attack", color: GameMessageEventArgs.ColorCategory.BattlePlayer);
                return;
            }

            _player.UseCurrentWeaponOn(_opponent);

            if (_opponent.IsAlive) AttackPlayer();
        }
        public void AttackPlayer() =>
            _opponent.UseCurrentWeaponOn(_player);

        public void Dispose()
        {
            _player.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnActionPerformed -= OnCombatantActionPerformed;
            _opponent.OnKilled -= OnOpponentKilled;
        }

        private void OnCombatantActionPerformed(object sender, string result) =>
            _messageBroker.RaiseMessage(result, color: GameMessageEventArgs.ColorCategory.Battle);

        private void OnOpponentKilled(object sender, System.EventArgs e)
        {
            _messageBroker.RaiseMessage($"\nYou have defeated {_opponent.Name}!", color: GameMessageEventArgs.ColorCategory.Battle);

            _messageBroker.RaiseMessage($"   You recieve {_opponent.RewardExperiencePoints} experience points", color: GameMessageEventArgs.ColorCategory.Battle);
            _player.AddExperience(_opponent.RewardExperiencePoints);

            _messageBroker.RaiseMessage($"   And {_opponent.Gold} gold.", color: GameMessageEventArgs.ColorCategory.Battle);
            _player.ReceiveGold(_opponent.Gold);

            _messageBroker.RaiseMessage($"   The monster drops:", color: GameMessageEventArgs.ColorCategory.Battle);

            foreach (GameItem item in _opponent.Inventory.Items)
            {
                _messageBroker.RaiseMessage($"     {StringGrammar.BeginsWithVowel(item.Name,true)}.", color: GameMessageEventArgs.ColorCategory.Battle);
                _player.AddItemToInventory(item);
            }

            OnCombatVictory?.Invoke(this, new CombatVictoryEventArgs());
        }
    }
}
