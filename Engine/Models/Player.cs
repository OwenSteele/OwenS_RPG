using Engine.Factories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Engine.Models
{
    /// <summary>
    /// To have the MainWindow.xaml auto update when the property is changed, need INotifyPropertyChanged. 
    /// INotifyPropertyChanged needs an event handler, PropertyChangedEventHandler, that invokes the change to the property from an event.
    /// But this means auto-properties cannot be used, need priavte variables and defined getters and setters
    /// </summary>
    public class Player : LivingEntity
    {
        #region properties and constructors
        private CharacterClass _characterClass;
        private int _experiencePoints;
        private int _nextLevelExperiencePoints = 100;
        private GroupedInventoryItem _currentGroupedConsumable;

        public CharacterClass CharacterClass
        {
            get => _characterClass; 
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }
        public GroupedInventoryItem CurrentGroupedConsumable
        {
            get =>  _currentGroupedConsumable;
            set
            {
                _currentGroupedConsumable = value;
                OnPropertyChanged();
            }
        }
        public int ExperiencePoints
        {
            get => _experiencePoints; 
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged();

                SetLevelAndMaximumHitPoints();
            }
        }
        public int NextLevelExperiencePoints
        {
            get => _nextLevelExperiencePoints; 
            private set
            {
                _nextLevelExperiencePoints = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ObservableCollection<QuestStatus> Quests { get; }
        public ObservableCollection<Recipe> Recipes { get; }

        public event EventHandler OnLeveledUp;
        public event EventHandler OnDexterityIncreased;
        public event EventHandler<string> OnNewRecipeLearnt;

        public Player(string name, int dexterity, int characterClassID, int experiencePoints,
                      int maximumHitPoints, int currentHitPoints, int gold) :
            base(name, dexterity, maximumHitPoints, currentHitPoints, gold)
        {
            CharacterClass = CharacterClassFactory.GetCharacterClassByID(characterClassID);
            ExperiencePoints = experiencePoints;

            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }

        #region Player Changes
        public void AddExperience(int experiencePoints) => ExperiencePoints += experiencePoints;

        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;
            Level = ExperiencePointsPerLevelCalculator();

            if (Level != originalLevel)
            {
                NextLevelExperiencePoints = ExperiencePointsPerLevelCalculator(ExperiencePoints);
                MaximumHitPoints = MaximumHitPointsPerLevelCalculator(originalLevel);
                Dexterity = DexterityPerLevelCalculator();

                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }

        private int ExperiencePointsPerLevelCalculator(int? experiencePoints = null)
        {
            int experiencePerLevel = 100;

            if (experiencePoints != null) return Level*experiencePerLevel;

            return (int)(Math.Floor(ExperiencePoints/100m))+1;
        }
        private int MaximumHitPointsPerLevelCalculator(int originalLevel)
        {
            int addtionalHitPoints = 100;
            return (MaximumHitPoints+ addtionalHitPoints);
        }
        private int DexterityPerLevelCalculator()
        {
            int currentDexterity = Dexterity;
            int initialClassDexterity = _characterClass.Dexterity;
            int currentLevel = Level + (initialClassDexterity - 3);

            //formula
            double firstTerm = Math.Pow(currentLevel * 13.88889, 0.45208);
            double secondTerm = -((currentLevel * currentLevel) * 4.363 / 10000) - 0.1;
            double calculatedDexterity = firstTerm + secondTerm;

            if (calculatedDexterity < currentDexterity) return currentDexterity;

            if (calculatedDexterity != currentDexterity)
                OnDexterityIncreased?.Invoke(this, System.EventArgs.Empty);

            return (int)calculatedDexterity;
        }

        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(x => x.ID == recipe.ID))
                Recipes.Add(recipe);

            OnNewRecipeLearnt?.Invoke(this,
                $"\nYou have learnt a new recipe, you can now make {StringGrammar.CheckIfPlural(recipe.Name, true)}.\n");
        }
        #endregion

    }
}
