using System.Collections.ObjectModel;
using Cyclops.Core.Model;
using Cyclops.Core.Mvvm;
using Cyclops.Core;

namespace Cyclops.Client.ViewModel
{
    public class ConferenceViewModel : ViewModelBase
    {
        private Conference conference;
        private string currentlyTypedMessage;

        public ConferenceViewModel(Conference conference)
        {
            Conference = conference;
            Conference.Banned += new System.EventHandler<Core.Model.CustomEventArgs.BannedEventArgs>(Conference_Banned);
            Conference.Kicked += new System.EventHandler<Core.Model.CustomEventArgs.KickedEventArgs>(Conference_Kicked);
            Conference.Disconnected += new System.EventHandler<Core.Model.CustomEventArgs.DisconnectEventArgs>(Conference_Disconnected);


            Messages = new ObservableCollection<MessageViewModel>();
            Conference.Messages.SynchronizeWith(Messages, msg => new MessageViewModel(msg));

            SendMessage = new RelayCommand(OnSendMessage, () => !string.IsNullOrEmpty(CurrentlyTypedMessage));
        }

        void Conference_Disconnected(object sender, Core.Model.CustomEventArgs.DisconnectEventArgs e)
        {
            AddSystemMessage("Disconnected due '{0}'", e.ErrorMessage);
        }

        void Conference_Kicked(object sender, Core.Model.CustomEventArgs.KickedEventArgs e)
        {
            AddSystemMessage("You was kicked due the reason: '{0}'", e.Reason);
        }

        void Conference_Banned(object sender, Core.Model.CustomEventArgs.BannedEventArgs e)
        {
            AddSystemMessage("You was banned due the reason: '{0}'", e.Reason);
        }

        private void AddSystemMessage(string messageFormat, params object[] args)
        {
            Messages.Add(new MessageViewModel(new SystemConferenceMessage
                                         {Text = string.Format(messageFormat, args), IsErrorMessage = true}));
        }

        public string CurrentlyTypedMessage
        {
            get { return currentlyTypedMessage; }
            set
            {
                currentlyTypedMessage = value;
                RaisePropertyChanged("CurrentlyTypedMessage");
            }
        }

        public RelayCommand SendMessage { get; private set; }

        public Conference Conference
        {
            get { return conference; }
            set
            {
                conference = value;
                RaisePropertyChanged("Conference");
            }
        }

        private ObservableCollection<MessageViewModel> messages;
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        private void OnSendMessage()
        {
            if (string.IsNullOrEmpty(CurrentlyTypedMessage))
                return;
            Conference.SendPublicMessage(CurrentlyTypedMessage);
            CurrentlyTypedMessage = string.Empty;
        }

        public override string ToString()
        {
            return Conference.ConferenceJid.User;
        }
    }
}