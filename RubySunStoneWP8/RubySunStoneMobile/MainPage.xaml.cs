using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using RubySunStoneMobile.Resources;
using System.Device.Location;
using RubySunStoneMobile.Utils;

namespace RubySunStoneMobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        static GeoCoordinateWatcher gcw;
        static GeoCoordinate position = new GeoCoordinate(43.607262969017, 6.983340382575);

        // Constructeur
        public MainPage()
        {
            InitializeComponent();

            ie.Source = new Uri(SettingsHelper.urlServeur(), UriKind.Absolute);
            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void ie_LoadCompleted(object sender, NavigationEventArgs e)
        {
            gcw = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            if (gcw.Permission == GeoPositionPermission.Granted)
            {
                gcw.MovementThreshold = 20;
                gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
                gcw.Start();
                gcw.StatusChanged += gcw_StatusChanged;
            }
            else
            {
                MessageBox.Show("Vous n'avez pas autorisé la permission de géolocalisation - Saisissez une position");
            }
        }

        //geolocalisation
        private void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            position = e.Position.Location;
            Etat.Text = position.Latitude + " " + position.Longitude;
            
            
        }
        private void gcw_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Etat.Text = e.Status.ToString();
        }

        private void Situer_Click(object sender, RoutedEventArgs e)
        {
            string lat = position.Latitude.ToString();
            string lng = position.Longitude.ToString();
            try
            {
                //ie.InvokeScript("essai");
            }
            catch (Exception ex)
            {
                Etat.Text = ex.Message;
            }
            //ie.InvokeScript("essai", new string[] { lat, lng });
            Dispatcher.BeginInvoke(() =>
            {
                //ie.InvokeScript("essai", new string[] { lat, lng });
                 NavigationService.Navigate(new Uri("/Views/AugmentedView.xaml", UriKind.Relative));
            });

        }



        // Exemple de code pour la conception d'une ApplicationBar localisée
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Définit l'ApplicationBar de la page sur une nouvelle instance d'ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Crée un bouton et définit la valeur du texte sur la chaîne localisée issue d'AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Crée un nouvel élément de menu avec la chaîne localisée d'AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}