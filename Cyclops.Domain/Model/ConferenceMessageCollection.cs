using System.Collections.ObjectModel;
using System.Linq;
using jabber;
using jabber.protocol.client;

namespace Cyclops.Core.Model
{
    public class ConferenceMessageCollection : ObservableCollection<ConferenceMessage>, IJabberSessionHolder
    {
        internal ConferenceMessageCollection(JabberSession session)
        {
            JabberSession = session;
        }

        public void Add(Message msg)
        {
            Add(new ConferenceMessage(JabberSession, msg));
        }

        public void Remove(JID authorJid)
        {
            var user = this.FirstOrDefault(i => i.Author == authorJid);
            if (user != null)
                Remove(user);
        }

        #region Implementation of IJabberSessionHolder

        /// <summary>
        /// Session object
        /// </summary>
        public JabberSession JabberSession { get; set; }

        #endregion
    }
}