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

namespace RubySunStoneMobile.ViewModels
{
    [DataContract]
    public class DisplayItemViewModel : DisplayItemViewModelBase
    {
        /// <summary>
        /// Provides display values for fields of the List, given its name. Also used for data binding to Display form UI
        /// </summary>
        public override object this[string spFieldName]
        {
            get
            {
                try
                {
                    return base[spFieldName];
                }
                catch (Exception)
                {
                    return "";                    
                }
            }
            set
            {
                if (value != null ) Debug.WriteLine("fieldName:" + spFieldName + " value:" + value.ToString());
                switch (spFieldName)
                {
                    case "Title":
                        Title = value.ToString();
                        break;
                    case "typebac":
                        typebac = value.ToString();
                        break;
                    case "ID":
                        Id = Convert.ToInt32(value.ToString());
                        break;
                    case "Geolocalisation":
                        GeoCoordonnee = (GeoCoordinate)value;
                        Distance = ARHelper.CalculateDistance((GeoCoordinate)value, MaPosition.GeoCoordonnee());
                        break;
                    case "Description":
                        Description = value.ToString();
                        break;
                    case "Collecte":
                        Collecte = value.ToString();
                        break;
                       
                }

                base[spFieldName] = value;
            }
        }
       
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
        /// Typebac
        /// </summary>
        private string bactype = "";

        public string typebac
        {
            get
            {
                return this.bactype;
            }

            set
            {
                if (this.bactype != value)
                {
                    this.bactype = value;
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
        /// Collecte
        /// </summary>
        private string collecte = "";

        public string Collecte
        {
            get
            {
                return this.collecte;
            }

            set
            {
                if (this.collecte != value)
                {
                    this.collecte = value;
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
        /// <summary>
        /// Initializes the ViewModel properties
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Deletes the current ListItem from SharePoint server
        /// </summary>
        public override void DeleteItem()
        {
            base.DeleteItem();
        }
        //public Object Tag { get; set; }
        public Microsoft.Phone.Maps.Controls.MapOverlay Tag { get; set; }
    }
}
