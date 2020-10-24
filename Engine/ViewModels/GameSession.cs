using Engine.Models;
using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.EventArgs;
using Engine.Services;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();

        #region player, location, world, monster | properties
        private Battle _currentBattle;
        private Location _currentLocation;
        private Monster _currentMonster;
        private Trader _currentTrader;
        private Quest _currentQuest;
        private List<Location> _currentMap;
        private List<KeyBind> _currentKeyBindings;
        private Player _currentPlayer { get; set; }

        private bool _playerMessageIsNotBlank;

        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnDexterityIncreased -= OnCurrentPlayerDexterityIncreased;
                    _currentPlayer.OnNewRecipeLearnt -= OnCurrentPlayerNewRecipeLearnt;
                }
                //with events -= means to unsubscribe from the event
                _currentPlayer = value;
                if (_currentPlayer != null)
                {
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnDexterityIncreased += OnCurrentPlayerDexterityIncreased;
                    _currentPlayer.OnNewRecipeLearnt += OnCurrentPlayerNewRecipeLearnt;
                }

                OnPropertyChanged();
            }
        }
        public Location CurrentLocation
        {
            get => _currentLocation;
            set
            {
                _currentLocation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));
                OnPropertyChanged(nameof(HasQuest));

                CompleteQuestAtLocation();
                GivePlayerQuestAtLocation();
                CurrentMonster = CurrentLocation.GetMonster();
                PlayerVisited();
                GiveItemsToPickUpAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
            }
        }
        public World CurrentWorld { get; }
        public Monster CurrentMonster
        {
            get => _currentMonster;
            set
            {
                if (_currentBattle != null)
                {
                    _currentBattle.OnCombatVictory -= OnCurrentMonsterKilled;
                    _currentBattle.Dispose();
                }

                _currentMonster = value;

                if (_currentMonster != null)
                {
                    _currentBattle = new Battle(CurrentPlayer, CurrentMonster);
                    _currentBattle.OnCombatVictory += OnCurrentMonsterKilled;
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }
        public Trader CurrentTrader
        {
            get => _currentTrader;
            set
            {
                _currentTrader = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }
        public Quest CurrentQuest
        {
            get => _currentQuest;
            set
            {
                _currentQuest = value;
                OnPropertyChanged();
            }
        }
        public List<Location> CurrentMap
        {
            get => _currentMap;
            set
            {
                _currentMap = value;
                OnPropertyChanged();
            }
        }
        public List<KeyBind> CurrentKeyBindings
        {
            get => _currentKeyBindings;
            set
            {
                _currentKeyBindings = value;
                OnPropertyChanged();
            }
        }
        public GameItem CurrentWeapon { get; set; }


        public List<CharacterClass> AllCharacterClasses { get; set; }
        public bool CloseAfterUserDetails { get; set; }
        public bool PlayerMessageIsNotBlank
        {
            get => _playerMessageIsNotBlank;
            set
            {
                _playerMessageIsNotBlank = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Has checks

        #region location
        public bool HasLocationToNorth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool HasLocationToWest =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToEast =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToSouth =>
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        public bool HasPlayerVisited =>
            CurrentWorld.LocationAt(0, 0).PlayerHasVisitedHere;

        #endregion

        #region monster, trader

        public bool HasMonster => CurrentMonster != null;
        public bool HasTrader => CurrentTrader != null;
        public bool HasQuest => CurrentQuest != null;
        #endregion

        public bool HasPlayerMessageToShow => PlayerMessageIsNotBlank;

        public bool HasPlayerMetQuestPrerequisites(Quest quest)
        {
            bool levelSatisfied = CurrentPlayer.Level >= quest.Prerequisites.level;
            bool itemsSatisfied = CurrentPlayer.Inventory.HasAllTheseItems(quest.Prerequisites.items);
            bool questsSatisfied = false;

            if (quest.Prerequisites.questIDs.Count != 0)
                foreach (int requiredQuestID in quest.Prerequisites.questIDs)
                {
                    questsSatisfied = CurrentPlayer.Quests.Any(
                        q => q.PlayerQuest.ID == requiredQuestID) ?
                        (CurrentPlayer.Quests.First(
                        q => q.PlayerQuest.ID == requiredQuestID).IsCompleted ? true : false) : false;
                }
            else questsSatisfied = true;

            if (levelSatisfied && itemsSatisfied && questsSatisfied) return true;
            else
            {
                _messageBroker.RaiseMessage($"\nYou cannot start the quest {quest.Name}.", color: GameMessageEventArgs.ColorCategory.Quest);
                if (!levelSatisfied) 
                    _messageBroker.RaiseMessage("-  You are not a high enough level.", color: GameMessageEventArgs.ColorCategory.Quest);
                
                if (!itemsSatisfied)
                {
                    _messageBroker.RaiseMessage("-  You are missing these items:", color: GameMessageEventArgs.ColorCategory.Quest);
                    foreach(ItemQuantity itemQuantity in quest.Prerequisites.items)
                    {
                        if (CurrentPlayer.Inventory.Items.FirstOrDefault(
                            i => i.ItemTypeID == itemQuantity.ItemID) is GameItem playerItem)
                        {
                           GroupedInventoryItem groupedInventoryItem = CurrentPlayer.Inventory.GroupedInventoryItems.FirstOrDefault(
                            gi => gi.Item.ItemTypeID == playerItem.ItemTypeID);

                            if(groupedInventoryItem.Quantity < itemQuantity.Quantity)
                                _messageBroker.RaiseMessage($"     {itemQuantity.Quantity - groupedInventoryItem.Quantity}x {playerItem.Name}",
                                        color: GameMessageEventArgs.ColorCategory.Quest);
                        }
                        else
                        {
                            GameItem gameItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);

                            if (gameItem.IsUnique)
                                _messageBroker.RaiseMessage($"     {StringGrammar.BeginsWithVowel(gameItem.Name, true)}",
                                    color: GameMessageEventArgs.ColorCategory.Quest);
                            else
                            _messageBroker.RaiseMessage($"     {itemQuantity.Quantity}x {gameItem.Name}",
                                color: GameMessageEventArgs.ColorCategory.Quest);
                        }
                    }
                }

                if(!questsSatisfied)
                {
                    _messageBroker.RaiseMessage($"-  You need to complete the following quests:",
                        color: GameMessageEventArgs.ColorCategory.Quest);

                    foreach (int requiredQuestID in quest.Prerequisites.questIDs)
                    {
                        if (!(CurrentPlayer.Quests.Any(
                            q => q.PlayerQuest.ID == requiredQuestID) ?
                            !CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == requiredQuestID).IsCompleted : false))
                        {
                            _messageBroker.RaiseMessage($"     {QuestFactory.GetQuestByID(requiredQuestID).Name}",
                                 color: GameMessageEventArgs.ColorCategory.Quest);
                        }
                    }
                }

                return false;
            }
        }          
          
        #endregion

        public GameSession()
        {
            CurrentPlayer = new Player
            (
                null,
                0,
                0,
                0,
                100,
                100,
                10
            );


            if (!CurrentPlayer.Inventory.Weapons.Any()) CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2002));

            CurrentWorld = WorldFactory.CreateWorld();

            CurrentLocation = CurrentWorld.LocationAt(0, 0);

            CurrentMap = CurrentWorld.GetAllLocations();

            AllCharacterClasses = CharacterClassFactory.CreateCharacterClasses();

            PlayerMessageIsNotBlank=true;
        }
        public void GameInitialise() =>
            _messageBroker.RaiseMessage($"\n\n\nWelcome to SOSCSRPG!\n\nPlease enjoy!\n\nFor the tutorial, click the 'Help' button, in the top right corner");

        #region movement
        public void MoveNorth()
        {
            if (HasLocationToNorth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
        }
        public void MoveWest()
        {
            if (HasLocationToWest)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
        }
        public void MoveEast()
        {
            if (HasLocationToEast)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
        }
        public void MoveSouth()
        {
            if (HasLocationToSouth)
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
        }
        public void PlayerVisited()
        {
            CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate).PlayerHasVisitedHere = true;
        }
        #endregion

        #region actions
        public void AttackCurrentMonster() =>
            _currentBattle.AttackOpponent();

        public void UseCurrentConsumable()
        {
            if (CurrentPlayer.CurrentGroupedConsumable != null)
            {
                GroupedInventoryItem currentGroupedInventoryItem = CurrentPlayer.CurrentGroupedConsumable;

                CurrentPlayer.CurrentConsumable = CurrentPlayer.CurrentGroupedConsumable.Item;

                CurrentPlayer.UseCurrentConsumable();

                CurrentPlayer.CurrentGroupedConsumable = CurrentPlayer.Inventory.Consumables.FirstOrDefault(
                    c => c.Item.ItemTypeID == currentGroupedInventoryItem.Item.ItemTypeID);
            }
        }

        public void CraftItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.Inventory.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);

                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        _messageBroker.RaiseMessage($"\n You have crafted {StringGrammar.BeginsWithVowelAndIsPlural(outputItem.Name)} {outputItem.Name}", color: GameMessageEventArgs.ColorCategory.Actions);
                    }
            }
            else
            {
                _messageBroker.RaiseMessage($"\nYou don't have the required ingredients to craft {StringGrammar.BeginsWithVowelAndIsPlural(recipe.Name)} {recipe.Name}");
                foreach (ItemQuantity ingredient in recipe.Ingredients)
                    _messageBroker.RaiseMessage($"{ingredient.Quantity}x {ItemFactory.ItemName(ingredient.ItemID)}.");
            }
        }
        #endregion

        #region 'At Location'
        private void CompleteQuestAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault
                    (q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.Inventory.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);

                        _messageBroker.RaiseMessage($"\nCongratulations! You have complete the {quest.Name} quest. " +
                            $"\nYou recieve {quest.RewardExperiencePoints} experience points" +
                            $" and {quest.RewardGold} gold.", color: GameMessageEventArgs.ColorCategory.Quest);
                        _messageBroker.RaiseMessage($"{quest.Name} completed!", true);
                        
                        CurrentPlayer.AddExperience(quest.RewardExperiencePoints);
                        CurrentPlayer.ReceiveGold(quest.RewardGold);

                        if (quest.RewardItems.Count>0)
                        {
                            string questRewardItemsMessage = null;
                            foreach (ItemQuantity itemQuantity in quest.RewardItems)
                            {
                                GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                                CurrentPlayer.AddItemToInventory(rewardItem);
                                questRewardItemsMessage += (rewardItem.Name + ", ");
                            }
                            _messageBroker.RaiseMessage(
                                $"You receive: {questRewardItemsMessage.Substring(0, questRewardItemsMessage.Length - 2)}.", color: GameMessageEventArgs.ColorCategory.Quest);
                        }
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }
        private void GivePlayerQuestAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    if (HasPlayerMetQuestPrerequisites(quest))
                    {
                        CurrentPlayer.Quests.Add(new QuestStatus(quest));

                        _messageBroker.RaiseMessage($"\nYou receive the '{quest.Name}' quest!", color: GameMessageEventArgs.ColorCategory.Quest);
                        _messageBroker.RaiseMessage($"New quest:\n{quest.Name}.", true);
                        _messageBroker.RaiseMessage(quest.Description, color: GameMessageEventArgs.ColorCategory.Quest);

                        if (quest.ItemsToComplete.Count > 0)
                        {
                            _messageBroker.RaiseMessage("Return with:", color: GameMessageEventArgs.ColorCategory.Quest);
                            foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
                            {
                                _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}", color: GameMessageEventArgs.ColorCategory.Quest);
                            }
                        }

                        _messageBroker.RaiseMessage("And you will receive:", color: GameMessageEventArgs.ColorCategory.Quest);
                        _messageBroker.RaiseMessage($"   {quest.RewardExperiencePoints} experience points", color: GameMessageEventArgs.ColorCategory.Quest);
                        _messageBroker.RaiseMessage($"   {quest.RewardGold} gold", color: GameMessageEventArgs.ColorCategory.Quest);
                        
                        foreach (ItemQuantity itemQuantity in quest.RewardItems)
                        {
                            _messageBroker.RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}", color: GameMessageEventArgs.ColorCategory.Quest);
                        }
                    }
                }

                CompleteQuestAtLocation();
            }
        }
        private void GiveItemsToPickUpAtLocation()
        {
            if (CurrentLocation.ItemsToPickUpHere != null)
            {
                foreach (GameItem item in CurrentLocation.ItemsToPickUpHere)
                {
                    CurrentPlayer.AddItemToInventory(item);
                    _messageBroker.RaiseMessage($"At {CurrentLocation.Name}, you find {StringGrammar.CheckIfPlural(item.Name,true)} on the floor.");
                }

                CurrentLocation.RemoveAllItemsToPickUp();
            }
        }
        #endregion

        #region UserDetails
        public void UpdateCurrentKeyBindings(bool leftHandedControls)=>
            CurrentKeyBindings = KeyBindingFactory.CreateUserInputKeys(leftHandedControls);
        public void UpdateCurrentPlayerName(string playerName) =>
            CurrentPlayer.Name = playerName;
        public void UpdateCurrentPlayerCharacterClass(string characterClassName)
        {
            CharacterClass characterClass = CharacterClassFactory.GetCharacterClassByName(characterClassName);
            CurrentPlayer.CharacterClass = characterClass;
            CurrentPlayer.Dexterity = characterClass.Dexterity;
        }


        public string CheckNameValidity(string input) =>
            UserDetailsMessage.CheckNameIsValid(input);

        public void UserDetailsOpenTutorialWindowOnLaunch(bool isEnabled) =>
            UserDetailsMessage.OpenTutorialWindowOnLaunch = isEnabled;
        public void UserDetailsLeftHandedControls(bool isEnabled) =>
            UserDetailsMessage.LeftHandedControls = isEnabled;

        public bool GetLeftHandedControlsState => UserDetailsMessage.LeftHandedControls;
        public bool GetOpenTutorialWindowOnLaunchState => UserDetailsMessage.OpenTutorialWindowOnLaunch;

        #endregion

        #region custom buttons
        public string ButtonNorth { get => ImageFactory.Button.North; } 
        public string ButtonWest { get => ImageFactory.Button.West; } 
        public string ButtonEast { get => ImageFactory.Button.East; } 
        public string ButtonSouth { get => ImageFactory.Button.South; } 
        public string ButtonTrader { get => ImageFactory.Button.Trader; } 
        public string ButtonMap { get => ImageFactory.Button.Map; } 
        public string ButtonAttack { get => ImageFactory.Button.Attack; } 
        public string ButtonUseConsumable { get => ImageFactory.Button.UseConsumable; } 
        public string ButtonInventoryTabQuests { get => ImageFactory.Button.UIInventoryTabQuests; } 
        public string ButtonInventoryTabBackpack { get => ImageFactory.Button.UIInventoryTabBackpack; } 
        public string ButtonInventoryTabRecipes { get => ImageFactory.Button.UIInventoryTabRecipe; } 
        public string ButtonInventoryTabRecipeCraft { get => ImageFactory.Button.InventoryTabRecipeCraft; } 

        #endregion

        #region custom UIBackgrounds

        #region main window
        public string BackgroundPlayerStatsCenter { get => ImageFactory.UIBackground.PlayerStatsCenter; } 
        public string BackgroundPlayerStatsEdge { get => ImageFactory.UIBackground.PlayerStatsEdge; } 
        public string BackgroundPlayerStatsGoldIcon { get => ImageFactory.UIBackground.PlayerStatsGoldIcon; } 
        public string BackgroundGameMessagesCenter { get => ImageFactory.UIBackground.GameMessagesCenter; } 
        public string BackgroundLocationBackgroundCenter { get => ImageFactory.UIBackground.LocationBackgroundCenter; } 
        public string BackgroundMonsterBackgroundCenter { get => ImageFactory.UIBackground.MonsterBackgroundCenterDefault; } 
        public string BackgroundMonsterBackgroundNameBanner { get => ImageFactory.UIBackground.MonsterBackgroundNameBanner; } 
        public string BackgroundActionPanelBackgroundCenter { get => ImageFactory.UIBackground.ActionPanelBackgroundCenter; } 
        #endregion

        #region map window
        public string MapBackgroundMainOverallBackground { get => ImageFactory.UIBackground.Map.MainOverallBackground; } 
        public string MapBackgroundCompass { get => ImageFactory.UIBackground.Map.MapScreenCompass; } 
        #endregion

        #region trader window
        public string TraderBackgroundMainOverallBackground { get => ImageFactory.UIBackground.Trader.MainOverallBackground; } 
        #endregion

        #region TutorialScreenUI
        public string TutorialKeyBindingsKeyBlankCenter {  get => ImageFactory.KeyboardKeys.BlackBlankCenter; } 
        public string TutorialKeyBindingsKeyBlankLeft { get => ImageFactory.KeyboardKeys.BlackBlankLeft; } 
        public string TutorialKeyBindingsKeyBlankRight { get => ImageFactory.KeyboardKeys.BlackBlankRight; } 
        #endregion

        #region UserDetailsUI
        public string UserDetailsMainOverallBackground { get => ImageFactory.UIBackground.UserDetails.MainOverallBackground; } 
        #endregion

        #endregion

        #region Event

        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            //lose gold on death
            int gold;
            if (CurrentPlayer.Gold < 50) gold = 10;
            else gold = (int)(CurrentPlayer.Gold * 0.25);
            CurrentPlayer.SpendGold(gold);

            if (CurrentMonster != null)
            {
                _messageBroker.RaiseMessage($"\n*****\nThe {CurrentMonster.Name} killed you\n You have lost {gold} gold..\n*****\n", color: GameMessageEventArgs.ColorCategory.Actions);
                _messageBroker.RaiseMessage($"You have been killed.",true);
            }
            else
            {
                _messageBroker.RaiseMessage($"You died.", true);
                _messageBroker.RaiseMessage($"\nYou died.", color:GameMessageEventArgs.ColorCategory.Actions);
            }

            CurrentLocation = CurrentWorld.LocationAt(0, -1);
            CurrentPlayer.CompletelyHeal();
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs) =>
            CurrentMonster = CurrentLocation.GetMonster();

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs) =>
            _messageBroker.RaiseMessage($"You are now level {CurrentPlayer.Level}!",true);

        private void OnCurrentPlayerDexterityIncreased(object sender, System.EventArgs eventArgs) =>
            _messageBroker.RaiseMessage($"You have gained one dexterity point!", true);

        private void OnCurrentPlayerNewRecipeLearnt(object sender, string result)
        {
            _messageBroker.RaiseMessage(result, color: GameMessageEventArgs.ColorCategory.Actions);
            _messageBroker.RaiseMessage("New Recipe!", true);
        }
        #endregion

    }
}
