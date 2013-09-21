using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Device.Location;

namespace RubySunStoneMobile.Model
{
    public class RubySunStoneDataContext : DataContext
    {
        // connection string.
        public RubySunStoneDataContext(string connectionString) : base(connectionString)
        { }

        // table pour les Palmiers.
        public Table<Palmier> Palmiers;

    }

    [Table]
    public class Palmier : INotifyPropertyChanged, INotifyPropertyChanging
    {
        // Id, clé primaire.
        private int _id;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL IDENTITY", CanBeNull = false, AutoSync = AutoSync.OnInsert)]//IsDbGenerated = false, , AutoSync = AutoSync.OnInsert)]
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    NotifyPropertyChanging("Id");
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }
        // Title
        private string title;

        [Column]
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    NotifyPropertyChanging("Title");
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        // Latitude
        private double _latitude;

        [Column]
        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude != value)
                {
                    NotifyPropertyChanging("Latitude");
                    _latitude = value;
                    NotifyPropertyChanged("Latitude");
                }
            }
        }
        // Longitude
        private double _longitude;

        [Column]
        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude != value)
                {
                    NotifyPropertyChanging("Longitude");
                    _longitude = value;
                    NotifyPropertyChanged("Longitude");
                }
            }
        }
        // etatPalmier
        private string _etatPalmier;

        [Column]
        public string etatPalmier
        {
            get { return _etatPalmier; }
            set
            {
                if (_etatPalmier != value)
                {
                    NotifyPropertyChanging("etatPalmier");
                    _etatPalmier = value;
                    NotifyPropertyChanged("etatPalmier");
                }
            }
        }
 
        // pour performance en update.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}