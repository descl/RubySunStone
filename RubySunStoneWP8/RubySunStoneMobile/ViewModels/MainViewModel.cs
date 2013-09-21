using juMP13.Models;
using juMP13.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Diagnostics;
using System.IO;
using System.Net;
using VDS.RDF;
using System.Linq;
using juMP13.dataprovence;
using System.Data.Services.Client;
using VDS.RDF.Ontology;
using VDS.RDF.Parsing;

namespace juMP13.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event Action dataLoaded;
        public event Action noData;
        public event DlProgressHandler DlProgress;

        private dataprovencetourismeDataService context;
        private readonly Uri dataprovenceUri = new Uri("http://dataprovence.cloudapp.net:8080/v1/dataprovencetourisme/");
        private DataServiceCollection<RestaurantsItem> restaurants;
        private DataServiceCollection<HotelsItem> hotels;

        public DataServiceCollection<HotelsItem> Hotels
        {
            get { return hotels; }
            set { hotels = value; }
        }

        public DataServiceCollection<RestaurantsItem> Restaurants
        {
            get { return restaurants; }
            set { restaurants = value; }
        }

        public delegate void DlProgressHandler(String text);


        public MainViewModel()
        {
            this._EventList = new ObservableCollection<EventModel>();
        }

        private ObservableCollection<EventModel> _EventList;
        public ObservableCollection<EventModel> EventList
        {
            get
            {
                return _EventList;
            }
        }

        /// <summary>
        /// propriété ViewModel MaPosition
        /// </summary>
        /// <returns></returns>
        
        private GeoCoordinate _maPosition;
        public GeoCoordinate MaPosition
        {
            get
            {
                return _maPosition;
            }
            set
            {
                if (_maPosition != value)
                {
                    _maPosition = value;
                    NotifyPropertyChanged("MaPosition");
                }
            }
        }
        /// <summary>
        /// Exemple de propriété qui retourne une chaîne localisée
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
             set;
        }

        void client_Progress(object sender, DownloadProgressChangedEventArgs e)
        {
            DlProgress("Téléchargement : " + e.BytesReceived / 1024 + "ko");
            Debug.WriteLine("Download Progress : " + e.BytesReceived);
        }

        void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                DlProgress("Téléchargement : complet !");
                MemoryStream stream = new MemoryStream();

                byte[] data = new byte[16 * 1024];
                int read;

                DlProgress("Analyse des évènements");

                while ((read = e.Result.Read(data, 0, data.Length)) > 0)
                {
                    stream.Write(data, 0, read);
                }
                OntologyGraph g = new OntologyGraph();
                stream.Flush();
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);

                String dataAsString = sr.ReadToEnd();
                StringParser.Parse(g, dataAsString);

                OntologyClass someClass = g.CreateOntologyClass(new Uri("http://data.mp2013.fr/Event"));
                IUriNode rdfType = g.CreateUriNode("rdf:type");

                EventList.Clear();
                
                foreach (OntologyClass c in someClass.SubClasses)
                {
                    Debug.WriteLine("Sub Class: " + c.Resource.ToString());
                    IEnumerable<Triple> ts = g.GetTriplesWithPredicateObject(rdfType, c.Resource);
                    foreach (Triple t in ts)
                    {
                        EventModel myEvent = new EventModel(t.Subject, c.Resource.ToString());
                        EventList.Add(myEvent);
                    }
                }

                {
                    IEnumerable<Triple> ts = g.GetTriplesWithPredicateObject(rdfType, someClass.Resource);
                    foreach (Triple t in ts)
                    {
                        EventModel myEvent = new EventModel(t.Subject, someClass.Resource.ToString());
                        EventList.Add(myEvent);
                    }
                }

                dataLoaded();

                stream.Close();
            }
            catch (Exception e1)
            {
                Debug.WriteLine(e1.Message);
                noData();
            }
        }

        /// <summary>
        /// Crée et ajoute quelques objets ItemViewModel dans la collection Items.
        /// </summary>
        public void LoadData()
        {
            context = new dataprovencetourismeDataService(dataprovenceUri);
            restaurants = new DataServiceCollection<RestaurantsItem>(context);
            hotels = new DataServiceCollection<HotelsItem>(context);

            var queryResto = from resto in context.Restaurants select resto;
            var queryHotel = from hotel in context.Hotels select hotel;

            restaurants.LoadCompleted += restaurants_LoadCompleted;
            hotels.LoadCompleted += hotels_LoadCompleted;

            restaurants.LoadAsync(queryResto);
            hotels.LoadAsync(queryHotel);
            
            OntologyGraph g = new OntologyGraph();
            try
            {
                System.DateTime today = System.DateTime.Today;
                String uriStr = "http://api.mp2013.fr/events?from=" + today.ToString("yyyy-MM-dd") + "&to=" + today.AddDays(7).ToString("yyyy-MM-dd") + "&token=285882861";
                Debug.WriteLine(uriStr);

                WebClient client = new SharpGIS.GZipWebClient();

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_Progress);
                client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
                client.OpenReadAsync(new Uri(uriStr));

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        void hotels_LoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            Debug.WriteLine("hotels loaded");
        }

        void restaurants_LoadCompleted(object sender, LoadCompletedEventArgs e)
        {
            Debug.WriteLine("restos loaded");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}