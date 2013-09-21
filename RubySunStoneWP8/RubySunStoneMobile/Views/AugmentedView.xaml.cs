using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Devices;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Matrix = Microsoft.Xna.Framework.Matrix;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Device.Location;
using RubySunStoneMobile.Utils;
using System.Diagnostics;
using RubySunStoneMobile.UserControls;
using RubySunStoneMobile.ViewModels;
using RubySunStoneMobile.Model;

namespace RubySunStoneMobile.Views
{
    public partial class AugmentedView : PhoneApplicationPage
    {
        PhotoCamera camera;
        Motion motion;

        Viewport viewport;  // "Fenêtre" de vue de la caméra
        Matrix projection;  // Matrice de projection de l'espace 3D sur l'écran
        Matrix view;        // Matrice de position de la caméra

        int WCSRadius = 10; //units
        int Radius = 500; //m

        List<Vector3> points; // Liste des points remarquables dans l'espace
        List<UIElement> AugmentedLabels; // Liste des éléments graphiques à afficher
        public AugmentedView()
        {
            InitializeComponent();
            points = new List<Vector3>();
            AugmentedLabels = new List<UIElement>();

        }
        public void InitViewport()
        {
            Debug.WriteLine("AugmentedView InitViewport deb:" + this.ActualWidth + " w-h " + this.ActualHeight);

            // Le viewport est la "fenêtre" à travers laquelle la caméra regarde, la caméra étant considérée 
            // comme un point représentant l'oeil de l'utilisateur.
            // hardcode KO: 728 w-h 480
            viewport = new Viewport(0, 0, (int)this.ActualWidth, (int)this.ActualHeight);

            float aspect = viewport.AspectRatio;

            // La matrice de projection sert à transformer les coordonnées 3D du monde en coordonnées 2D sur l'écran
            projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 12);

            // La matrice View représente la caméra, l'oeil de l'utilisateur
            // La caméra pointe dans le sens de l'axe des Z, au centre de l'écran, la direction vers le haut étant l'axe des X
            view = Matrix.CreateLookAt(Vector3.UnitZ, Vector3.Zero, Vector3.UnitX);
            //Dispatcher.BeginInvoke(() => AddPalmiers()); 
            Debug.WriteLine("AugmentedView InitViewport fin");
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //Debug.WriteLine("AugmentedView OnNavigatedTo deb");
            //App.MainViewModel.ViewDataLoaded += new EventHandler<ViewDataLoadedEventArgs>(OnViewDataLoaded);
            //App.MainViewModel.InitializationCompleted += new EventHandler<InitializationCompletedEventArgs>(OnViewModelInitialization);
            //this.DataContext = App.MainViewModel;

            //App.BacModel.ViewDataLoaded += new EventHandler<ViewDataLoadedEventArgs>(OnViewDataLoaded);
            //App.BacModel.InitializationCompleted += new EventHandler<InitializationCompletedEventArgs>(OnViewModelInitialization);
            this.DataContext = App.PalmierModel;

            #region Initialisation de la caméra, et affichage à l'écran
            camera = new PhotoCamera(CameraType.Primary);//.FrontFacing);
            viewfinderBrush.SetSource(camera);
            #endregion

            #region Initialisation de l'API Motion

            if (!Motion.IsSupported)
            {
                MessageBox.Show("Motion API Not supported :(");
                return;
            }

            if (motion == null)
            {
                motion = new Motion();
                motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(66); // 15 FPS
                motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(motion_CurrentValueChanged);

                try
                {
                    motion.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossible de démarrer l'API Motion! " + ex.Message);
                }
            }
            #endregion

            //Debug.WriteLine("AugmentedView OnNavigatedTo fin");
            AddPalmiers();

        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Debug.WriteLine("AugmentedView OnNavigatedFrom deb");
            //App.BacModel.ViewDataLoaded -= new EventHandler<ViewDataLoadedEventArgs>(OnViewDataLoaded);
            //App.BacModel.InitializationCompleted -= new EventHandler<InitializationCompletedEventArgs>(OnViewModelInitialization);           
            Debug.WriteLine("AugmentedView OnNavigatedFrom fin");
        }
        void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            // This event arrives on a background thread. Use BeginInvoke
            // to call a method on the UI thread.
            Dispatcher.BeginInvoke(() => CurrentValueChanged(e.SensorReading));
        }

        private void CurrentValueChanged(MotionReading reading)
        {
            try
            {
                MotionReading motionReading = reading;

                //if (motion.IsDataValid)
                //{
                //    // Show the numeric values for attitude.
                //    Debug.WriteLine("YAW: " + MathHelper.ToDegrees(motionReading.Attitude.Yaw).ToString("0") + "° PITCH: " + MathHelper.ToDegrees(motionReading.Attitude.Pitch).ToString("0") + "° ROLL: " + MathHelper.ToDegrees(motionReading.Attitude.Roll).ToString("0") + "°");
                //}

                if (viewport.Width == 0) InitViewport();

                // Position du device == Matrice de position du Motion Sensor * Rotation à 90% sur l'axe X (le téléphone étant tenu horizontalement)
                Matrix attitude = Matrix.CreateRotationX(MathHelper.PiOver2) * motionReading.Attitude.RotationMatrix;

                for (int i = 0; i < points.Count; i++)
                {
                    // Positionnement de chaque point dans le repère XNA 3D. Z pointe devant, X vers le haut
                    Matrix world = Matrix.CreateWorld(points[i], Vector3.UnitZ, Vector3.UnitX);

                    // Projection du point de l'espace 3D dans le repère 2D de l'écran
                    Vector3 projected = viewport.Project(Vector3.Zero, projection, view, world * attitude);

                    // N'afficher que les Palmiers qui sont devant le téléphone.
                    //Statut.Text = "Motion points:" + points.Count.ToString() + " i:" + i.ToString() + " proj X:" + projected.X.ToString() + " proj Y:" + projected.Y.ToString() + " proj Z:" + projected.Z.ToString();
                    if (projected.Z > 1 || projected.Z < 0)
                    {
                        AugmentedLabels[i].Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        AugmentedLabels[i].Visibility = System.Windows.Visibility.Visible;

                        // Centrage du contrôle sur le point
                        TranslateTransform tt = new TranslateTransform();
                        tt.X = projected.X - (AugmentedLabels[i].RenderSize.Width / 2);
                        tt.Y = projected.Y - (AugmentedLabels[i].RenderSize.Height / 2);
                        AugmentedLabels[i].RenderTransform = tt;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("erreur viewport.Width?" + ex.Message);
                Statut.Text = "erreur viewport.Width";
            }
        }

        private void Capture_Click(object sender, EventArgs e)
        {
            CaptureScreen.capture(100, true, MaPosition.GeoCoordonnee().Latitude, MaPosition.GeoCoordonnee().Longitude);
        }

        private void Charger_Click(object sender, EventArgs e)
        {
            //DistancePanel.Visibility = System.Windows.Visibility.Visible;
            DistanceBtn.Visibility = System.Windows.Visibility.Visible;
            Distance.Visibility = System.Windows.Visibility.Visible;
        }
        private void AddPalmiers()
        {
            //if (App.BacModel.GetPalmiersList() != null)
            //{
            Debug.WriteLine("GetPalmiersList contient des elements");
            Statut.Text = "Ajout des Palmiers";
            try
            {
                Etat.Text = "Nb palmiers: " + App.PalmierModel.GetPalmiersList().Count;
       
                foreach (RubySunStoneMobile.Model.Palmier palmier in App.PalmierModel.GetPalmiersList())
                {
                    Pushpin pushpin = new Pushpin(palmier);
                    // Calcul du cap du palmier en fonction de la position du téléphone
                    double Bearing = Math.Round(ARHelper.CalculateBearing(pushpin.GeoCoordonnee, MaPosition.GeoCoordonnee()), 0);
                    Debug.WriteLine("bearing:" + Bearing);
                    // Calcul de la position du palmier en fonction de l'angle, et de la distance, dans le repère XNA
                    Vector3 PositionBacRelative = ARHelper.AngleToVector(Bearing, (WCSRadius * ARHelper.CalculateDistance(pushpin.GeoCoordonnee, MaPosition.GeoCoordonnee()) / Radius));

                    AddPalmier(PositionBacRelative, pushpin);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AddPalmiers, erreur:" + ex.Message);
                Statut.Text = "Erreur ajout Palmiers:" + ex.Message;

            }
            //}
            //else
            //{
            //    Debug.WriteLine("GetPalmiersList == null");
            //    Statut.Text = "Palmiers pas encore chargés";
            //}
        }
        void AddPalmier(Vector3 position, Pushpin pushpin)
        {
            Debug.WriteLine("AugmentedView AddBac deb, distance: " + pushpin.Distance);
            Int64 maximumDistance = Convert.ToInt64(Distance.Text);
            if (pushpin.Distance < maximumDistance && pushpin.Distance > 1)
            {
                Debug.WriteLine("AugmentedView position X: " + position.X + " Y:" + position.Y + " Z:" + position.Z);
                PalmierControl bc = new PalmierControl();
                bc.Opacity = 0.7;// 3000/(pushpin.Distance+1);//
                bc.DataContext = pushpin;
                //bc.Width = 100000 / (pushpin.Distance + 1);

                points.Add(position);

                LayoutRoot.Children.Add(bc);
                AugmentedLabels.Add(bc);
                Statut.Text = "Ajout du palmier, distance: " + pushpin.Distance + " position X: " + position.X + " Y:" + position.Y + " Z:" + position.Z;
            }


            Debug.WriteLine("AugmentedView AddPalmier fin:" + points.Count.ToString());
        }
        private void deletePalmiers()
        {
            points.RemoveAll(TousVector3);
            AugmentedLabels.ForEach(delegate(UIElement bc)
            {
                LayoutRoot.Children.Remove(bc);
            });


            AugmentedLabels.RemoveAll(TousUIElement);
            points = new List<Vector3>();
            AugmentedLabels = new List<UIElement>();
        }
        private static bool TousVector3(Vector3 v)
        {
            return true;
        }
        private static bool TousUIElement(UIElement u)
        {
            return true;
        }
        private void Distance_Click(object sender, RoutedEventArgs e)
        {
            //DistancePanel.Visibility = System.Windows.Visibility.Collapsed;
            DistanceBtn.Visibility = System.Windows.Visibility.Collapsed;
            Distance.Visibility = System.Windows.Visibility.Collapsed;

            deletePalmiers();
            AddPalmiers();
        }
    }
}