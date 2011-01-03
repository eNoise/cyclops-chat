using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Cyclops.Client.Configuration;
using Cyclops.Client.Properties;
using Cyclops.Core.Model;
using Cyclops.Core.Mvvm;
using jabber;

namespace Cyclops.Client.ViewModel
{
    /// <summary>
    /// </summary>
    public class MainViewModel : ViewModelBase, IJabberSessionHolder
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

            if (IsInDesignMode)
            {
                return;
            }
            //OnUpdateStyles();

            //TODO: replace hard-coded config
            JabberSession = new JabberSession(Dispatcher.CurrentDispatcher);
            ConferencesModels = new ObservableCollection<ConferenceViewModel>();
            JabberSession.BeginAuthentication(new ConnectionConfig
                                                    {
                                                        User = "cyclops",
                                                        Server = "jabber.uruchie.org",
                                                        Password = "cyclops"
                                                    });

            JabberSession.Conferences.Add(new JID("main", "conference.jabber.uruchie.org", "CIA"));
            JabberSession.Conferences.Add(new JID("cyclops", "conference.jabber.uruchie.org", "CIA2"));
            JabberSession.Conferences.Add(new JID("CIA", "conference.jabber.uruchie.org", "CIA2"));
            JabberSession.Conferences.Add(new JID("support", "conference.jabber.uruchie.org", "CIA2"));
            JabberSession.Conferences.SynchronizeWith(ConferencesModels, conference => new ConferenceViewModel(conference));
            

            UpdateStyles = new RelayCommand(OnUpdateStyles);
        }

        private void OnUpdateStyles()
        {
            using (FileStream fs = new FileStream(@"Themes\Default\OutputAreaStyles.xaml", FileMode.Open))
            {
                ResourceDictionary rd = XamlReader.Load(fs) as ResourceDictionary;
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(rd);
            }
        }

        public RelayCommand UpdateStyles { get; private set; }


        #region Implementation of IJabberSessionHolder

        private JabberSession jabberSession;

        /// <summary>
        /// Session object
        /// </summary>
        public JabberSession JabberSession
        {
            get { return jabberSession; }
            private set
            {
                jabberSession = value;
                RaisePropertyChanged("JabberSession");
            }
        }

        #endregion

        private ObservableCollection<ConferenceViewModel> conferencesModels;

        /// <summary>
        /// Collection of currently opened conferences models
        /// </summary>
        public ObservableCollection<ConferenceViewModel> ConferencesModels
        {
            get { return conferencesModels; }
            set
            {
                conferencesModels = value;
                RaisePropertyChanged("ConferencesModels");
            }
        }

    }
}