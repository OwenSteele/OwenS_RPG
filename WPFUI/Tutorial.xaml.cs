using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Engine.Models;
using Engine.ViewModels;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for Tutorial.xaml
    /// </summary>
    public partial class Tutorial : Window
    {
        public GameSession Session => DataContext as GameSession;

        public Tutorial()
        {
            InitializeComponent();
        }

        public void OnLoad_Tutorial(object sender, RoutedEventArgs e)
        {
            foreach(KeyBind key in Session.CurrentKeyBindings)
            {
                object wantedNode = KeyBindingsTab.FindName(key.KeyAction+"_Key");
                if (wantedNode is TextBlock)
                {
                    TextBlock wantedChild = wantedNode as TextBlock;
                    wantedChild.Text=key.ID;
                }
            }
        }

        public void OnClick_Close(object sender, RoutedEventArgs e) => Close();
    }
}
