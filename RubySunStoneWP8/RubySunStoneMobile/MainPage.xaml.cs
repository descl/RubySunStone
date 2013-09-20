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
           
            ie.Source = new Uri("http://www.kevs3d.co.uk/dev/phoria/", UriKind.Absolute);
            // Exemple de code pour la localisation d'ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void ie_LoadCompleted(object sender, NavigationEventArgs e)
        {
            ie.InvokeScript("essai", "ok");
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