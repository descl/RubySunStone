using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Input;
using System.Diagnostics;
using RubySunStoneMobile.ViewModels;
using System.Windows.Navigation;

namespace RubySunStoneMobile.UserControls
{
    public partial class PalmierControl : UserControl
    {
        private bool Etendu = false;
        
        public PalmierControl()
        {
            InitializeComponent();
        }
        private void LayoutRoot_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Canvas.SetZIndex(this, 99);
            Debug.WriteLine("tap");
            if (Etendu)
            {
                this.Width = 140;
                this.Height = 90;
            }
            else
            {
                this.Width = 380;
                this.Height = 140;
            }
            
            Etendu = !Etendu;
        }
        
        //private void LayoutRoot_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        //{
        //    Canvas.SetZIndex(this, 99);
        //    Debug.WriteLine("doubletap:" + this.LayoutRoot.Name);

        //    //Set selected Item in MainViewModel
        //    App.MainViewModel.SelectedItemDisplayViewModelInstance = (sender as Grid).DataContext as DisplayItemViewModel;

        //    Tribul_mobile.App.RootFrame.Navigate(new Uri("/Views/DisplayForm.xaml", UriKind.Relative));
        //}
    }
}
