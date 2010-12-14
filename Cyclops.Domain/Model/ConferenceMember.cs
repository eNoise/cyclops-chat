using System;
using System.ComponentModel;
using jabber;
using jabber.connection;
using jabber.protocol.iq;

namespace Cyclops.Core.Model
{
    public class ConferenceMember
    {
        private readonly RoomParticipant participant;
        private readonly Room room;

        internal ConferenceMember(RoomParticipant participant, Room room)
        {
            this.participant = participant;
            this.room = room;

            room.OnParticipantPresenceChange += room_OnParticipantPresenceChange;
        }

        /// <summary>
        /// Members nick
        /// </summary>
        public string Nick
        {
            get { return participant.Nick; }
        }

        public bool IsModer
        {
            get
            {
                return Affiliation == RoomAffiliation.admin || Affiliation == RoomAffiliation.owner ||
                       Role == RoomRole.moderator;
            }
        }

        /// <summary>
        /// Members presence type
        /// </summary>
        public string StatusText
        {
            get
            {
                if (participant != null && participant.Presence != null)
                    return participant.Presence.Status;
                return string.Empty;
            }
        }

        public string StatusType
        {
            get
            {
                if (participant != null && participant.Presence != null)
                    return participant.Presence.Show;
                return string.Empty;
            }
        }

        private static Random random = new Random(Environment.TickCount);

        /// <summary>
        /// URL to user avatar image file
        /// </summary>
        public string AvatarUrl
        {
            get
            {
                return string.Format(@"C:\Avatars\{0}.jpg", random.Next(1, 8));
            }
        }

        /// <summary>
        /// Conference JID with nick
        /// </summary>
        public JID NickJid
        {
            get { return participant.NickJID; }
        }

        /// <summary>
        /// Members actual JID
        /// </summary>
        public JID RealJid
        {
            get { return participant.RealJID; }
        }

        /// <summary>
        /// Role
        /// </summary>
        public RoomRole Role
        {
            get { return participant.Role; }
        }

        /// <summary>
        /// Affiliation
        /// </summary>
        public RoomAffiliation Affiliation
        {
            get { return participant.Affiliation; }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        private void room_OnParticipantPresenceChange(Room room, RoomParticipant participant)
        {
            if (this.participant == participant)
                foreach (var property in GetType().GetProperties())
                    OnPropertyChanged(property.Name); //Request for UI to update all member fields
        }

        /// <summary>
        /// Return nick by default
        /// </summary>
        public override string ToString()
        {
            return Nick;
        }
    }
}