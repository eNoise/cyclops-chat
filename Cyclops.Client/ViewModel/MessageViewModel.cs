using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Cyclops.Client.MessageDecoration;
using Cyclops.Core.Model;
using Cyclops.Core.Mvvm;

namespace Cyclops.Client.ViewModel
{
    /// <summary>
    /// View model for message
    /// </summary>
    public class MessageViewModel : ViewModelBase
    {
        public MessageViewModel(IConferenceMessage msg)
        {
            RawMessage = msg;
            Paragraph = MessagePresenter.Present(msg);
        }

        public IConferenceMessage RawMessage { get; private set; }
        public Paragraph Paragraph { get; private set; }

        public override string ToString()
        {
            return RawMessage.ToString();
        }
    }
}
