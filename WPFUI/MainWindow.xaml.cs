using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Engine.EventArgs;
using Engine.ViewModels;
using Engine.Models;
using WPFUI;
using Engine.Services;
using System.Threading.Tasks;
using System.Linq;
using System.Windows.Media;

namespace GameEngine
{
    /// <summary>
    /// https://www.youtube.com/watch?v=hiaD2mi5Lm8&list=PLvt_WhOWCiEi0GEpvlv34PPsmvVlmpi9u&index=26
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameSession _gameSession = new GameSession();
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();
        private static List<string> _playerMessagesTimed = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            _messageBroker.OnMessageRaised += OnGameMessageRaised;

            _messageBroker.OnPlayerMessageRaised += OnPlayerMessageRaised;
            DataContext = _gameSession; //xaml will use data context
        }

        #region user input keys
        public void InitializeUserInputActions()
        {
            List<Key> userInputActionKeys = new List<Key>();

            foreach(KeyBind keyID in _gameSession.CurrentKeyBindings)
                userInputActionKeys.Add((Key)Enum.Parse(typeof(Key), keyID.ID));


            _userInputActions.Add(userInputActionKeys[0], () => _gameSession.MoveNorth());
            _userInputActions.Add(userInputActionKeys[1], () => _gameSession.MoveWest());
            _userInputActions.Add(userInputActionKeys[2], () => _gameSession.MoveSouth());
            _userInputActions.Add(userInputActionKeys[3], () => _gameSession.MoveEast());
            _userInputActions.Add(userInputActionKeys[4], () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(userInputActionKeys[5], () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(userInputActionKeys[6], () => OnClick_DisplayTraderScreen());
            _userInputActions.Add(userInputActionKeys[7], () => OnClick_DisplayMapScreen());
            _userInputActions.Add(userInputActionKeys[8], () => OnClick_DisplayTutorialScreen());

            userInputActionKeys.Clear();

        }
        public void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(_userInputActions.ContainsKey(e.Key))
                _userInputActions[e.Key].Invoke();

        }
            #endregion
        public void OnLoad_MainWindow(object sender, RoutedEventArgs e)
        {
            _gameSession.GameInitialise();

            InitializeUserDetails();

            PlayerStatsGrid.Visibility = Visibility.Visible;

            InitializeUserInputActions();

            if (_gameSession.GetOpenTutorialWindowOnLaunchState)
                OnClick_DisplayTutorialScreen();
        }
        #region OnClick Events
        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)=>
            _gameSession.MoveNorth();
        private void OnClick_MoveWest(object sender, RoutedEventArgs e)=>
            _gameSession.MoveWest();
        private void OnClick_MoveEast(object sender, RoutedEventArgs e)=>
            _gameSession.MoveEast();
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)=>
            _gameSession.MoveSouth();
        private void OnClick_AttackMonster(object sender, RoutedEventArgs e)=>
            _gameSession.AttackCurrentMonster();
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            //Paragraph lastParagraph = GameMessages.Document.Blocks.Last() as Paragraph;
            //lastParagraph.
            //if (GameMessages.Document.Blocks.Last().ToString() == e.Message) ;

            Paragraph paragraph = new Paragraph(new Run(e.Message));
            var converter = new BrushConverter();
            paragraph.Foreground = (Brush)converter.ConvertFromString(e.Color);
            GameMessages.Document.Blocks.Add(paragraph);
            GameMessages.ScrollToEnd();
        }
        private async void OnPlayerMessageRaised(object sender, PlayerMessageEventArgs e)
        {
            _playerMessagesTimed.Add(e.Message);

            while (_playerMessagesTimed.Any())
            {
                //when _playerMessages is not empty: make RTB visible and print all messages in separate paragraphs
                //remove each message from list after alloted task time, then reprint.
                PlayerMessages.Visibility = Visibility.Visible;
                PlayerMessages.Document.Blocks.Clear();

                foreach(string message in _playerMessagesTimed)
                    PlayerMessages.Document.Blocks.Add(new Paragraph(new Run($">> {message}")));

                await Task.Delay(7000);

                if(_playerMessagesTimed.Any())
                    _playerMessagesTimed.RemoveAt(0);
            }

            PlayerMessages.Document.Blocks.Clear();
            PlayerMessages.Visibility = Visibility.Hidden;
        }
        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();

            if (_gameSession.CurrentPlayer.CurrentConsumable != null)
                ComboBoxConsumables.SelectedItem = _gameSession.CurrentPlayer.CurrentGroupedConsumable;
        }
        private void OnClick_DisplayTraderScreen(object sender=null, RoutedEventArgs e=null)
        {
            if (_gameSession.CurrentTrader != null)
            {
                TradeScreen tradeScreen = new TradeScreen();
                tradeScreen.Owner = this;
                tradeScreen.DataContext = _gameSession;
                tradeScreen.ShowDialog();
            }
            //stops user interaction with main window
        }
        private void OnClick_DisplayMapScreen(object sender = null, RoutedEventArgs e = null)
        {
            Map map = new Map();
            map.Owner = this;
            map.DataContext = _gameSession;
            map.ShowDialog();
        }
        private void OnClick_DisplayTutorialScreen(object sender = null, RoutedEventArgs e = null)
        {
            Tutorial tutorial = new Tutorial();
            tutorial.Owner = this;
            tutorial.DataContext = _gameSession;
            tutorial.ShowDialog();
        }
        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
           Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }

        private void InitializeUserDetails()
        {
            UserDetails userDetails = new UserDetails();
            userDetails.DataContext = _gameSession;
            userDetails.Owner = this;
            userDetails.ShowDialog();
            if (_gameSession.CloseAfterUserDetails == true) ShutDownCalled();
        }

        internal void ShutDownCalled()
        {
            foreach(Window window in App.Current.Windows)
            {
                if (window != App.Current.MainWindow)
                    window.Close();
            }
            App.Current.MainWindow.Close();
            App.Current.Shutdown(0);
            Environment.Exit(0);
        }
        #endregion
    }

}
