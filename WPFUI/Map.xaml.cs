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
using Engine.ViewModels;
using Engine.Models;
using WPFUI.CustomConverters;

namespace GameEngine
{
    /// <summary>
    /// Interaction logic for Map.xaml
    /// </summary>
    public partial class Map : Window
    {
        public GameSession Session => DataContext as GameSession;
        public Map()
        {
            InitializeComponent();
        }
    public void OnLoad_MapLocalGrid(object sender, RoutedEventArgs e)
        {
            int xCurrent = Session.CurrentLocation.XCoordinate;
            int yCurrent = Session.CurrentLocation.YCoordinate;

            for (int row = 2; row >= -2; row--)
                for (int column = -2; column <= 2; column++)
                {
                    StackPanel mapLocation = new StackPanel();
                    TextBlock textBlockTop = new TextBlock();
                    Image mapLocationImage = new Image();
                    BitmapImage mapLocationBitmapImage = new BitmapImage();
                    TextBlock textBlockBottom = new TextBlock();
                    Location currentMapLocation = Session.CurrentWorld.LocationAt(xCurrent + column, yCurrent + row);

                    mapLocation.Orientation = Orientation.Vertical;
                    mapLocation.HorizontalAlignment = HorizontalAlignment.Center;
                    mapLocation.Height = 100;
                    mapLocation.Width = 100;

                    if (currentMapLocation != null && currentMapLocation.PlayerHasVisitedHere)
                    { 
                        textBlockTop.Text = currentMapLocation.Name;
                        textBlockTop.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlockTop.TextWrapping = TextWrapping.WrapWithOverflow;

                        mapLocationBitmapImage = FileToBitmapConverter.GetLocationBitmapImage(currentMapLocation.ImageFileName);

                        mapLocationImage.Source = mapLocationBitmapImage;
                        mapLocationImage.MaxHeight = 60;
                        mapLocationImage.MaxWidth = 60;

                        textBlockBottom.Text = "";
                        string locationInfo = null;
                        if (Session.CurrentLocation == currentMapLocation)
                        {
                            textBlockBottom.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Red);
                            locationInfo = "You are here";
                        }
                        else
                        {
                            textBlockBottom.Foreground = new System.Windows.Media.SolidColorBrush(Colors.Blue);
                            if (currentMapLocation.QuestsAvailableHere.Count != 0) locationInfo += " Q ";
                            if (currentMapLocation.MonstersHere.Count != 0) locationInfo += " M ";
                            if (currentMapLocation.TraderHere != null) locationInfo += " T ";

                        }
                        textBlockBottom.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlockBottom.FontWeight = FontWeights.Bold;
                        textBlockBottom.Text = locationInfo;

                        mapLocation.Children.Add(textBlockTop);
                        mapLocation.Children.Add(mapLocationImage);
                        mapLocation.Children.Add(textBlockBottom);
                    }
                    else if (column == -2 && row == 2)
                    {
                        mapLocationBitmapImage.BeginInit();
                        mapLocationBitmapImage.UriSource = new Uri(Session.MapBackgroundCompass, UriKind.Relative);
                        mapLocationBitmapImage.EndInit();

                        mapLocationImage.Source = mapLocationBitmapImage;
                        mapLocationImage.MaxHeight = 100;
                        mapLocationImage.MaxWidth = 100;

                        mapLocation.Children.Add(mapLocationImage);
                    }
                    else mapLocation.Opacity = 0;// new System.Windows.Media.SolidColorBrush(Colors.Black);

                    MapGrid.Children.Add(mapLocation);
                    Grid.SetRow(mapLocation, 2-row);
                    Grid.SetColumn(mapLocation, column+2);
                }
        }
    public void OnClick_Close(object sender, RoutedEventArgs e) => Close();
    }
}
