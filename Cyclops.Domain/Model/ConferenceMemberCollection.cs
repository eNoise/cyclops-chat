using System;
using System.Linq;
using System.Collections.ObjectModel;
using jabber;
using jabber.connection;

namespace Cyclops.Core.Model
{
    public class ConferenceMemberCollection : ObservableCollection<ConferenceMember>, IJabberSessionHolder
    {
        internal ConferenceMemberCollection(JabberSession session)
        {
            JabberSession = session;
        }

        public void Add(RoomParticipant participant, Room room)
        {
            if (this.Any(i => i.NickJid == participant.NickJID))
                return;
            Add(new ConferenceMember(participant, room));
        }

        public void Remove(JID nickJid)
        {
            var user = this.FirstOrDefault(i => i.NickJid == nickJid);
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