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
using Engine.EventArgs;

namespace GameEngine
{
    /// <summary>
    /// Interaction logic for MapScreen.xaml
    /// </summary>
    public partial class MapScreen : Window
    {
        public GameSession Session => DataContext as GameSession;
        public MapScreen()
        {
            OnLoad_Map();
            InitializeComponent();
        }
    public void OnLoad_Map()
        {
            Grid mapGrid = new Grid();

            //add the rows and columns
            for (int i = 0; i< 4; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1,GridUnitType.Star);
                ColumnDefinition columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(1, GridUnitType.Star);

                mapGrid.RowDefinitions.Add(rowDef);
                mapGrid.ColumnDefinitions.Add(columnDef);
            }
            mapGrid.ShowGridLines = true;
            for (int currentColumn = 0; currentColumn < 5; currentColumn++)
                for (int currentRow = 0; currentRow < 4; currentRow++)
                {
                    TextBlock textBlock = new TextBlock();
                    try
                    {
                        Location mapLocationToLoad = new Location();
                        mapLocationToLoad = Session.CurrentWorld.LocationAt(2-currentColumn,currentRow-2);

                        if (mapLocationToLoad!= null)
                        {
                            textBlock.Text = mapLocationToLoad.Name;

                            textBlock.FontSize = 12;
                            textBlock.FontWeight = FontWeights.Bold;
                           //Grid.SetRow(textBlock, 2- ;
                            Grid.SetColumn(textBlock, mapLocationToLoad.XCoordinate+2 );

                            if (textBlock.Text != null) textBlock.Visibility = Visibility.Visible;
                            else textBlock.Visibility = Visibility.Hidden;

                            mapGrid.Children.Add(textBlock);
                        }
                    }
                    catch { }
                };
        }
    public void OnClick_Close(object sender, RoutedEventArgs e) => Close();
    }
}
