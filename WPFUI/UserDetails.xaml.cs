using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using Engine.ViewModels;
using GameEngine;
using Engine.Models;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for UserDetails.xaml
    /// </summary>
    public partial class UserDetails : Window
    {
        private static string PlayerName { get; set; }
        private static string CharacterClassName { get; set; }

        public string UserDetailsMainOverallBackground { get => Session.UserDetailsMainOverallBackground; } 

        private static bool CompletedUserDetails = false;
        public GameSession Session => DataContext as GameSession;
        public UserDetails()
        {
            InitializeComponent();
            NameTextBox.Text = "Owen";
        }

        [Obsolete("Set to obselete, when userdetails is not to be skipped.", true)]
        public void TEMPSKIP(object sender, RoutedEventArgs e)
        {
            ////Temp skip!
            //PlayerName = "Owen"; CompletedUserDetails = true;
            //this.Session.CurrentPlayer.Name = PlayerName;
            //Session.UserDetailsOpenTutorialWindowOnLaunch(CheckBoxOpenTutorial.IsEnabled); //checking the tutorial window
            //Close();
            ////Temp skip!
        }
        public void OnClick_CheckNameValidity(object sender, RoutedEventArgs e)
        {
            NameCheckResultMessage.Text = Session.CheckNameValidity(NameTextBox.Text);
            if (NameCheckResultMessage.Text.Contains("Valid"))
            {
                PlayerName = NameTextBox.Text;
                ComboBoxCharacterClass.Visibility = Visibility.Visible;
            }
            else
            {
                ComboBoxCharacterClass.Visibility = Visibility.Hidden;
            }
        }
        public void OnSelectionChanged_ComboBoxCharacterClass(object sender, SelectionChangedEventArgs e)
        {
            if (NameCheckResultMessage.Text.Contains("Valid"))
            {
                CharacterClass characterClass = ComboBoxCharacterClass.SelectedItem as CharacterClass;
                CharacterClassName = characterClass.Name;
                ButtonStart.Visibility = Visibility.Visible;
            }
            else
            {
                ButtonStart.Visibility = Visibility.Hidden;
            }
        }
        public void OnClick_UserDetailsComplete(object sender=null, RoutedEventArgs e=null)
        {
            if (PlayerName != null)//ComboBoxCharacterClass.SelectedItem != null
            {
                CompletedUserDetails = true;
                Session.UpdateCurrentPlayerName(PlayerName);
                Session.UpdateCurrentPlayerCharacterClass(CharacterClassName);
                Session.UpdateCurrentKeyBindings(CheckBoxLefthanded.IsChecked ?? false);
            }
            Session.UserDetailsLeftHandedControls(CheckBoxLefthanded.IsChecked ?? false);
            Session.UserDetailsOpenTutorialWindowOnLaunch(CheckBoxOpenTutorial.IsChecked ?? false);

            Close();
        }
        public void OnUserDetails_Closing(object sender, CancelEventArgs e)
        {
            if (!CompletedUserDetails)
            {
                string[] message = new string[2]; //caption, message
                if (PlayerName == null)
                {
                    message[0] = "You can't be nameless!";
                    message[1] = "You have not set a player name!\nDo you want to exit instead?";
                }
                else
                {
                    message[0] = "Exit?";
                    message[1] = "This will exit the game completely, are you sure?";
                }

                MessageBoxResult result = MessageBox.Show
                        (
                        message[1],
                        message[0],
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning
                        );
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
                else Session.CloseAfterUserDetails =true;
            }
        }
    }
}
