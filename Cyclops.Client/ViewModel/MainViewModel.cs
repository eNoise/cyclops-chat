using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using Cyclops.Client.Properties;
using Cyclops.Core.Model;
using Cyclops.Core.MVVM;
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
            OnUpdateStyles();

            JabberSession = new JabberSession(Dispatcher.CurrentDispatcher);
            ConferencesModels = new ObservableCollection<ConferenceViewModel>();
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                JabberSession.BeginAuthentication(new ConnectionConfig
                                                      {
                                                          User = "cyclops",
                                                          Server = "jabber.uruchie.org",
                                                          //NetworkHost = "10.1.1.135",
                                                          Password = "cyclops"
                                                      });

                JabberSession.Conferences.Add(new JID("main", "conference.jabber.uruchie.org", "CIA"));
                //мультиконфовость пока не пашет (там из-за DataTemplat'инга в TabControl)
                //JabberSession.Conferences.Add(new JID("cyclops", "conference.jabber.uruchie.org", "CIA2"));
                //JabberSession.Conferences.Add(new JID("CIA2", "conference.jabber.uruchie.org", "CIA2"));
                //JabberSession.Conferences.Add(new JID("support", "conference.jabber.uruchie.org", "CIA2"));
                JabberSession.Conferences.SynchronizeWith(ConferencesModels, conference => new ConferenceViewModel(conference));

                // Code runs "for real"
            }



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