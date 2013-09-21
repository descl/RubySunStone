using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RubySunStoneMobile.Utils;

namespace RubySunStoneMobile.Model
{
    public class Pushpin : Palmier
    {
        //constructeur
        public Pushpin(Palmier palmier)
        {
            this.geoCoordonnee = new GeoCoordinate(palmier.Latitude, palmier.Longitude);
            this.distance = ARHelper.CalculateDistance(this.GeoCoordonnee, MaPosition.GeoCoordonnee());
            this.Id = palmier.Id;
            this.Title = palmier.Title;
            this.etatPalmier = palmier.etatPalmier;
            this.Latitude = palmier.Latitude;
            this.Longitude = palmier.Longitude;
            this.pictureUrl = "/Assets/PalmTree_" + palmier.etatPalmier + ".png";
        }
        private string pictureUrl;

        public string PictureUrl
        {
            get
            {
                return this.pictureUrl;
            }
        }
        /// <summary>
        /// Geolocalisation
        /// </summary>
        private GeoCoordinate geoCoordonnee;

        public GeoCoordinate GeoCoordonnee
        {
            get
            {
                return this.geoCoordonnee;
            }
        }

        /// <summary>
        /// Distance
        /// </summary>
        private int distance;

        public int Distance
        {
            get
            {
                return this.distance;
            }        
        }
    }
}
