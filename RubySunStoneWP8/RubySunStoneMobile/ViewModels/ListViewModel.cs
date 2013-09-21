using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Phone.Application;
using Microsoft.SharePoint.Client;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using RubySunStoneMobile.Utils;
using System.Diagnostics;

namespace RubySunStoneMobile.ViewModels
{
    [DataContract]
    [KnownType(typeof(ListViewModelBase))]
    [KnownType(typeof(ListDataProvider))]
    [KnownType(typeof(ObservableCollection<DisplayItemViewModel>))]
    public class ListViewModel : ListViewModelBase
    {
        /// <summary>
        /// Provides access to the DisplayItem ViewModel of selected item.
        /// </summary>
        [DataMember]
        public DisplayItemViewModel SelectedItemDisplayViewModelInstance { get; set; }

        /// <summary>
        /// Makes sure that all the artifacts (eg. ListSchema, DataProvider etc.) required by ViewModel has been initialized properly. 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads List data for SharePoint View with the specified name
        /// </summary>
        /// <param name="viewName">Name of the SharePoint View which has to be loaded</param>
        public void LoadData(string viewName, params object[] filterParameters)
        {
            Debug.WriteLine("Charge données view");// + MaPosition.GeoCoordonnee().Longitude);

            displayViewModelCollection = new ObservableCollection<DisplayItemViewModel>();
            IsBusy = true;
            DataProvider.LoadData(viewName, OnLoadViewDataCompleted, filterParameters);
        }

        /// <summary>
        /// Refreshes List data for SharePoint View with the specified name.
        /// </summary>
        /// <param name="viewName">Name of the SharePoint View which has to be loaded</param>
        public void RefreshData(string viewName, params object[] filterParameters)
        {
            IsBusy = true;
            ((ListDataProvider)DataProvider).RefreshData(viewName, OnLoadViewDataCompleted, filterParameters);
        }

        /// <summary>
        /// Code to execute when a SharePoint View has been loaded completely.
        /// </summary>
        /// <param name="e" />
        private void OnLoadViewDataCompleted(LoadViewCompletedEventArgs e)
        {
            IsBusy = false;
            if (e.Error != null)
            {
                OnViewDataLoaded(this, new ViewDataLoadedEventArgs { ViewName = e.ViewName, Error = e.Error });
                return;
            }

            //Create a collection of DisplayItemViewModels
            //ObservableCollection<DisplayItemViewModel> displayViewModelCollection = new ObservableCollection<DisplayItemViewModel>();
            // dans loaddata displayViewModelCollection = new ObservableCollection<DisplayItemViewModel>();

            foreach (ListItem item in e.Items)
            {
                DisplayItemViewModel displayViewModel = new DisplayItemViewModel { ID = item.Id.ToString(), DataProvider = this.DataProvider };
                Debug.WriteLine("OnLoadViewDataCompleted:" + item.Id.ToString());
                displayViewModel.Initialize();
                try
                {
                    displayViewModelCollection.Add(displayViewModel);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("OnLoadViewDataCompleted, erreur:" + ex.Message);
                }
            }


            OnViewDataLoaded(this, new ViewDataLoadedEventArgs { ViewName = e.ViewName, ViewData = displayViewModelCollection });
        }
        private ObservableCollection<DisplayItemViewModel> _displayViewModelCollection;
        public ObservableCollection<DisplayItemViewModel> displayViewModelCollection
        {
            get
            {
                return _displayViewModelCollection;
            }
            set
            {
                if (_displayViewModelCollection != value)
                {
                    _displayViewModelCollection = value;
                    //NotifyPropertyChanged("Pushpinlist");
                }
            }
        }

        public ObservableCollection<DisplayItemViewModel> GetPushpinlist()
        {
            return displayViewModelCollection;
        }

    }
}
