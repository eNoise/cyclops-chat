using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cyclops.Client.ViewModel;

namespace Cyclops.Client
{
    /// <summary>
    /// Interaction logic for ConferenceView.xaml
    /// </summary>
    public partial class ConferenceView : UserControl
    {
        public ConferenceView()
        {
            InitializeComponent();
            DataContext = this;
        }



        public ConferenceViewModel ConferenceViewModel
        {
            get { return (ConferenceViewModel)GetValue(ConferenceViewModelProperty); }
            set { SetValue(ConferenceViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConferenceViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConferenceViewModelProperty =
            DependencyProperty.Register("ConferenceViewModel", typeof(ConferenceViewModel), typeof(ConferenceView), new UIPropertyMetadata(null));

        
    }
}
