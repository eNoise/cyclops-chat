using System.Collections.ObjectModel;
using System.Linq;
using jabber;

namespace Cyclops.Core.Model
{
    public class ConferenceCollection : ObservableCollection<Conference>, IJabberSessionHolder
    {
        public ConferenceCollection(JabberSession session)
        {
            JabberSession = session;
        }

        public Conference Add(JID conferenceJid)
        {
            if (this.Any(i => i.ConferenceJid == conferenceJid))
                return this.First(i => i.ConferenceJid == conferenceJid);

            var conference = new Conference(JabberSession, conferenceJid);
            Add(conference);
            return conference;
        }

        public void Remove(JID confernceJid)
        {
            Remove(this.First(i => i.ConferenceJid == confernceJid));
        }

        protected override void InsertItem(int index, Conference item)
        {
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Leave();
            base.RemoveItem(index);
        }

        #region Implementation of IJabberSessionHolder

        /// <summary>
        /// Session object
        /// </summary>
        public JabberSession JabberSession { get; private set; }

        #endregion
    }
}