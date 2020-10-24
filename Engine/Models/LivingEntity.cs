using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Engine.Services;

namespace Engine.Models
{
    public abstract class LivingEntity : BaseNotificationClass
    {

        #region Name/HP/Gold variables and properties
        //abstract = can never be instantiated
        private string _name;
        private int _dexterity;
        private int _currentHitPoints;
        private int _maximumHitPoints;
        private int _gold;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;
        private Inventory _inventory;

        public string Name
        {
            get => _name; 
            set // only settable in this class, LivingEntity
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public int Dexterity
        {
            get => _dexterity;
            set
            {
                if (value > 18) _dexterity = 18;
                else if (value <3) _dexterity = 3;
                else _dexterity = value;

                OnPropertyChanged();
            }
        }
        public int CurrentHitPoints
        {
            get => _currentHitPoints; 
            //private
            set
            {
                _currentHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int MaximumHitPoints
        {
            get => _maximumHitPoints; 
            //protected 
                set //can be set by the child classes too, that are based on LivingEntity
            {
                _maximumHitPoints = value;
                OnPropertyChanged();
            }
        }
        public int Gold
        {
            get => _gold; 
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }
        public int Level
        {
            get => _level; 
            protected set //can be set by the child classes too, that are based on LivingEntity
            {
                _level = value;
                OnPropertyChanged();
            }
        }
        public GameItem CurrentWeapon
        {
            get => _currentWeapon; 
            set
            {
                //on weapon change, unsub, change then resub
                if (_currentWeapon != null)
                    _currentWeapon.Action.OnActionPerformed -= RaiseOnActionPerformedEvent;
                _currentWeapon = value;
                if (_currentWeapon != null)
                    _currentWeapon.Action.OnActionPerformed += RaiseOnActionPerformedEvent;

                OnPropertyChanged();
            }
        }
        public GameItem CurrentConsumable
        {
            get => _currentConsumable;
            set
            {
                if(_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseOnActionPerformedEvent;
                }
                _currentConsumable = value;
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseOnActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }
        public Inventory Inventory
        {
            get => _inventory;
            private set
            {
                _inventory = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Inventory Setup
        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

        public bool IsAlive => CurrentHitPoints > 0;
        public bool IsDead => !IsAlive;

        public event EventHandler<string> OnActionPerformed;
        public event EventHandler OnKilled;

        protected LivingEntity(string name, int dexterity, int maximumHitPoints, int currentHitPoints, int gold, int level =1)
        {
            Name = name;
            Dexterity = dexterity;
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Level = level;

            Inventory = new Inventory();
        }

        #endregion

        #region UseCurrent
        public void UseCurrentWeaponOn(LivingEntity target) =>
            CurrentWeapon.PerformAction(this, target);

        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }

        #endregion

        #region Changes to entity functions
        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHitPoints -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHitPoints = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hitPointsToHeal)
        {
            CurrentHitPoints += hitPointsToHeal;

            if (CurrentHitPoints > MaximumHitPoints)
                CurrentHitPoints = MaximumHitPoints;
        }

        public void CompletelyHeal()
        {
            CurrentHitPoints = MaximumHitPoints;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }

        public void SpendGold(int amountOfGold)
        {
            if (amountOfGold > Gold)
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold} gold, and cannot spend {amountOfGold} gold");

            Gold -= amountOfGold;
        }
        #endregion

        #region Add/Remove Inventory
        public void AddItemToInventory(GameItem item)
        {

            if(item.Category == GameItem.ItemCategory.Recipe && this.GetType() == typeof(Player))
            {
                var currentPlayer = this as Player;
                currentPlayer.LearnRecipe(RecipeFactory.RecipeByID(item.RecipeID));
            }
            else
                Inventory = Inventory.AddItem(item);

            if (item.Category == GameItem.ItemCategory.Weapon && _currentWeapon == null && Inventory.Weapons.Count == 1)
                _currentWeapon = item;
        }

        public void RemoveItemFromInventory(GameItem item) =>
            Inventory = Inventory.RemoveItem(item);

        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities) =>
            Inventory = Inventory.RemoveItems(itemQuantities);
        #endregion

        #region Events
        private void RaiseOnKilledEvent() =>
            OnKilled?.Invoke(this, new System.EventArgs());

        private void RaiseOnActionPerformedEvent(object sender, string result) =>
            OnActionPerformed?.Invoke(this, result);

#endregion
    }
}
