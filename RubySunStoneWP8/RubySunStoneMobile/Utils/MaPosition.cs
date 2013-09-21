using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubySunStoneMobile.Utils
{
    public static class MaPosition
    {
        static GeoCoordinateWatcher gcw;
        static GeoCoordinate position = new GeoCoordinate(43.607262969017, 6.983340382575);
        //constructeur
        static MaPosition()
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
                //MessageBox.Show("Vous n'avez pas autorisé la permission de géolocalisation - Saisissez une position");
            }
        }

        static void gcw_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Debug.WriteLine(e.Status.ToString());
        }
        
        public static GeoCoordinate GeoCoordonnee()
        {
            return position;
        }
        
        //geolocalisation
        private static void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            position = e.Position.Location;
            Debug.WriteLine(position.Latitude + " " + position.Longitude);
        }   
    }
}
