using System;
using System.Device.Location;
using Microsoft.Xna.Framework;
using System.Diagnostics;
namespace RubySunStoneMobile.Utils
{
    public static class ARHelper
    {
        private const int rayonTerre =  6367445;
        //avec R le rayon de la terre 6 367 445 mètres
        //a = latA
        //b = latB
        //c = lonA
        //d = lonB
        //Rcos-1( sin(a)*sin(b)+cos(a)*cos(b)*cos(c-d) )
       // ACOS(
        //SIN(RADIANS(@latitude_origem))*SIN(RADIANS (@latitude_destino))
        //+ COS(RADIANS(@latitude_origem))*COS(RADIANS(@latitude_destino))*COS(RADIANS(@dif_Longitude)))
           
        public static int CalculateDistance(GeoCoordinate Element, GeoCoordinate MyPosition)
        {
            double num1 = Math.Sin(ARHelper.DegreeToRadian(MyPosition.Latitude) ) * Math.Sin(ARHelper.DegreeToRadian(Element.Latitude) );
            double num2 = Math.Cos(ARHelper.DegreeToRadian(MyPosition.Longitude - Element.Longitude));
            double num3 = Math.Cos(ARHelper.DegreeToRadian(MyPosition.Latitude)) * Math.Cos(ARHelper.DegreeToRadian(Element.Latitude)) * num2;
            double num4 = num1 + num3;
            return Convert.ToInt32(rayonTerre * Math.Acos(num4));
        }
        public static double CalculateBearing(GeoCoordinate Element, GeoCoordinate MyPosition)
        {
            ARHelper.DegreeToRadian(MyPosition.Latitude - Element.Latitude);

            double num1 = ARHelper.DegreeToRadian(MyPosition.Longitude - Element.Longitude);
            double num2 = ARHelper.DegreeToRadian(Element.Latitude);
            double num3 = ARHelper.DegreeToRadian(MyPosition.Latitude);
            double angle = Math.Atan2(Math.Sin(num1) * Math.Cos(num3), Math.Cos(num2) 
                * Math.Sin(num3) - Math.Sin(num2) * Math.Cos(num3) * Math.Cos(num1));
            
            return ARHelper.RadianToDegree(angle) + 180.0;
        }

        public static Vector3 AngleToVector(double inAngle, double inRadius)
        {
            double num = ARHelper.DegreeToRadian(inAngle - 90.0);
            return new Vector3((float)Math.Round(inRadius * Math.Cos(num)), 0.0f, (float)Math.Round(inRadius * Math.Sin(num)));
        }

        public static double DegreeToRadian(double angle)
        {
            return 3.14159265358979 * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * 57.2957795130823;
        }
        //pas testé
        //public double GetDistanceTo(GeoCoordinate other)
        //{
        //    if (double.IsNaN(this.Latitude) || double.IsNaN(this.Longitude) || double.IsNaN(other.Latitude) || double.IsNaN(other.Longitude))
        //    {
        //        throw new ArgumentException(SR.GetString("Argument_LatitudeOrLongitudeIsNotANumber"));
        //    }
        //    else
        //    {
        //        double latitude = this.Latitude * 0.0174532925199433;
        //        double longitude = this.Longitude * 0.0174532925199433;
        //        double num = other.Latitude * 0.0174532925199433;
        //        double longitude1 = other.Longitude * 0.0174532925199433;
        //        double num1 = longitude1 - longitude;
        //        double num2 = num - latitude;
        //        double num3 = Math.Pow(Math.Sin(num2 / 2), 2) + Math.Cos(latitude) * Math.Cos(num) * Math.Pow(Math.Sin(num1 / 2), 2);
        //        double num4 = 2 * Math.Atan2(Math.Sqrt(num3), Math.Sqrt(1 - num3));
        //        double num5 = 6376500 * num4;
        //        return num5;
        //    }
        //}
    }
}
