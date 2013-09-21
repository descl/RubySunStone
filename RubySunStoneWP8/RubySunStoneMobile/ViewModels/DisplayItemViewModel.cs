using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RubySunStoneMobile.Utils;
using System.ComponentModel;

namespace RubySunStoneMobile.ViewModels
{
    [DataContract]
    public class DisplayItemViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Geolocalisation
        /// </summary>
        private GeoCoordinate geoCoordinate;

        public GeoCoordinate GeoCoordonnee
        {
            get
            {
                return this.geoCoordinate;
            }

            set
            {
                if (this.geoCoordinate != value)
                {
                    this.geoCoordinate = value;
                    //this.NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// etatPalmier
        /// </summary>
        private string _etatPalmier = "";

        public string etatPalmier
        {
            get
            {
                return this._etatPalmier;
            }

            set
            {
                if (this._etatPalmier != value)
                {
                    this._etatPalmier = value;
                    //this.NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Distance
        /// </summary>
        private int distance = 1;

        public int Distance
        {
            get
            {
                return this.distance;
            }

            set
            {
                if (this.distance != value)
                {
                    this.distance = value;
                    //this.NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Description
        /// </summary>
        private string description = "";

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    //this.NotifyPropertyChanged();
                }
            }
        }
        /// <summary>
        /// Description
        /// </summary>
        private int id = 0;

        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    //this.NotifyPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Title
        /// </summary>
        private string title = "";

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    //this.NotifyPropertyChanged();
                }
            }
        }
        
        //public Object Tag { get; set; }
        public Microsoft.Phone.Maps.Controls.MapOverlay Tag { get; set; }
    }
}
