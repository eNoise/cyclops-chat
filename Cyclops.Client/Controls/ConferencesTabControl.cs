using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Cyclops.Client.ViewModel;

namespace Cyclops.Client.Controls
{
    public class ConferencesTabControl : TabControl
    {
        public ObservableCollection<ConferenceViewModel> ConferencesSource
        {
            get { return (ObservableCollection<ConferenceViewModel>)GetValue(ConferencesSourceProperty); }
            set { SetValue(ConferencesSourceProperty, value); }
        }

        void ConferencesSourceCollectionChanged(object sender,  NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                AddConference(e.NewItems[0] as ConferenceViewModel);
        }

        private void SubscribeToSourceChanges()
        {
            if (ConferencesSource == null)
                return;
            ConferencesSource.CollectionChanged += ConferencesSourceCollectionChanged;
            ConferencesSource.ForEach(i => AddConference(i));
        }

        private void AddConference(ConferenceViewModel conference)
        {
            Items.Add(new TabItem() {Content = new ConferenceView { ConferenceViewModel = conference}, Header = conference.ToString()});
        }

        // Using a DependencyProperty as the backing store for ConferencesSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConferencesSourceProperty =
            DependencyProperty.Register("ConferencesSource", typeof(ObservableCollection<ConferenceViewModel>),
            typeof(ConferencesTabControl), new PropertyMetadata(ConferencesSourceCollectionChangedStatic));

        private static void ConferencesSourceCollectionChangedStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tabcontrol = d as ConferencesTabControl;
            tabcontrol.SubscribeToSourceChanges();
        }
    }
}
