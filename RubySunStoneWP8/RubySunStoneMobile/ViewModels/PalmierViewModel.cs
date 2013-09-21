using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using RubySunStoneMobile.Model;


namespace RubySunStoneMobile.ViewModels
{
    public class PalmierViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private RubySunStoneDataContext palmierDB;

        // Class constructor, create the data context object.
        public PalmierViewModel(string RubySunStoneDBConnectionString)
        {
            palmierDB = new RubySunStoneDataContext(RubySunStoneDBConnectionString);
        }

        // Tous Les Palmiers.
        private ObservableCollection<Palmier> _tousLesPalmiers;
        public ObservableCollection<Palmier> TousLesPalmiers
        {
            get { return _tousLesPalmiers; }
            set
            {
                _tousLesPalmiers = value;
                NotifyPropertyChanged("TousLesPalmiers");
            }
        }
        // Query database and load the collections and list used by the pivot pages.
        public void LoadPalmiersFromDatabase()
        {

            // query for tous les palmiers.
            var palmiersInDB = from Palmier palmier in palmierDB.Palmiers
                                select palmier;

            // Query tous palmiers.
            TousLesPalmiers = new ObservableCollection<Palmier>(palmiersInDB);

        }
        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            palmierDB.SubmitChanges();
        }
        // Add .
        public void AjoutBac(Palmier palmier)
        {
            //Supprime avant insert
            var palmierASupprimer = from Palmier delpalmier in palmierDB.Palmiers
                                where delpalmier.Title == palmier.Title
                                select delpalmier;
            palmierDB.Palmiers.DeleteAllOnSubmit(palmierASupprimer);
            palmierDB.SubmitChanges();
            // Insere
            palmierDB.Palmiers.InsertOnSubmit(palmier);
             
            palmierDB.SubmitChanges();

            TousLesPalmiers.Add(palmier);
        }
        // del .
        public void SupprimePalmier(Palmier palmier)
        {
            TousLesPalmiers.Remove(palmier);
            palmierDB.Palmiers.DeleteOnSubmit(palmier);
            palmierDB.SubmitChanges();
        }

        public ObservableCollection<Palmier> GetPalmiersList()
        {
            return TousLesPalmiers;
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the app that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion


    }
}