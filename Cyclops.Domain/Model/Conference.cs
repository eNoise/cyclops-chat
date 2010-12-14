using System;
using System.ComponentModel;
using System.Linq;
using Cyclops.Core.Model.CustomEventArgs;
using Cyclops.Core.Model.Enums;
using Cyclops.Core.Resources;
using jabber;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Core.Model
{
    /// <summary>
    /// Represents a single conference
    /// </summary>
    public sealed class Conference : INotifyPropertyChanged, IJabberSessionHolder
    {
        private bool isConnected;
        private Room room;

        internal Conference(JabberSession session, JID conferenceJid)
        {
            if (conferenceJid == null)
                throw new ArgumentNullException("conferenceJid");

            JabberSession = session;
            JabberSession.ConnectionDropped += OnConnectionDropped;
            ConferenceJid = conferenceJid;

            Members = new ConferenceMemberCollection(session);
            Messages = new ConferenceMessageCollection(session);

            //if we are currently authenticated - then lets join to the channel imidiatly
            if (JabberSession.IsAuthenticated)
                Authenticated(this, new AuthenticationEventArgs());

            JabberSession.Authenticated += Authenticated;
        }

        private void OnConnectionDropped(object sender, AuthenticationEventArgs e)
        {
            room = null;
            IsConnected = false;
            Disconnected(this, new DisconnectEventArgs(ConnectionErrorKind.ConnectionError, e.ErrorMessage));
        }

        /// <summary>
        /// True, if we successfully entered to the conference
        /// </summary>
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                RaisePropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// Observable collection of members
        /// </summary>
        public ConferenceMemberCollection Members { get; private set; }

        /// <summary>
        /// Observable collection of messages
        /// </summary>
        public ConferenceMessageCollection Messages { get; private set; }

        /// <summary>
        /// Conference identifier
        /// </summary>
        public JID ConferenceJid { get; private set; }
        
        private void Authenticated(object sender, AuthenticationEventArgs e)
        {
            room = JabberSession.ConferenceManager.GetRoom(ConferenceJid);
            room.OnJoin += r => JabberSession.Invoke(() => room_OnJoin(r));
            room.OnLeave += (r, p) => JabberSession.Invoke(() => room_OnLeave(r, p));
            room.OnSubjectChange += room_OnSubjectChange;
            room.OnPresenceError += (r, p) => JabberSession.Invoke(() => room_OnPresenceError(r, p));

            room.OnSelfMessage += (s, msg) => JabberSession.Invoke(() => room_OnSelfMessage(s, msg));
            room.OnAdminMessage += (s, msg) => JabberSession.Invoke(() => room_OnAdminMessage(s, msg));
            room.OnRoomMessage += (s, msg) => JabberSession.Invoke(() => room_OnRoomMessage(s, msg));

            room.OnParticipantJoin += (r, p) => JabberSession.Invoke(() => room_OnParticipantJoin(r, p));
            room.OnParticipantLeave += (r, p) => JabberSession.Invoke(() => room_OnParticipantLeave(r, p));
            room.Join();
        }

        private void room_OnPresenceError(Room room, Presence pres)
        {
            if (pres.Error == null)
                return;

            switch (pres.Error.Code)
            {
                case 409: //conflict
                    Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.NickConflict,
                                                             ErrorMessageResources.NickConflictErrorMessage));
                    break;

                case 403: //banned
                    Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.Banned,
                                                             ErrorMessageResources.BannedErrorMessage));
                    break;

                case 401: //not-auth
                    Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.PasswordRequired,
                                                             ErrorMessageResources.PasswordRequiredErrorMessage));
                    break;

                    //captcha...
            }
        }
        
        private void room_OnLeave(Room room, Presence pres)
        {
            if (!pres.IsNullOrEmpty() && pres["x"] != null)
            {
                var userX = pres["x"] as UserX;
                if (!userX.Status.IsNullOrEmpty())
                {
                    if (userX.Status.Any(i => i == RoomStatus.KICKED))
                        Kicked(this, new KickedEventArgs(null, userX.RoomItem.Reason));
                    else if (userX.Status.Any(i => i == RoomStatus.BANNED))
                        Banned(this, new BannedEventArgs(null, userX.RoomItem.Reason));
                }
            }
            Members.Clear();
            IsConnected = false;
        }

        /// <summary>
        /// Leave the conference
        /// </summary>
        public void Leave(string reason = "")
        {
            if (IsConnected)
                room.Leave(reason);
        }

        void room_OnJoin(Room room)
        {
            Joined(this, new ConferenceJoinEventArgs());
            IsConnected = true;
            RoomParticipant meAsParticipant = null;
            foreach (RoomParticipant participant in room.Participants)
            {
                //if (participant.NickJID == Co)
                Members.Add(participant, room);
            }


            //Members.Add(room.Participants.P, room);
        }

        public event EventHandler<DisconnectEventArgs> Disconnected = delegate { };

        private string subject;
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;
                RaisePropertyChanged("Subject");
            }
        }

        private JID subjectAuthor;
        public JID SubjectAuthor
        {
            get { return subjectAuthor; }
            set
            {
                subjectAuthor = value;
                RaisePropertyChanged("SubjectAuthor");
            }
        }


        private void room_OnParticipantLeave(Room room, RoomParticipant participant)
        {
            Members.Remove(participant.NickJID);
        }

        private void room_OnParticipantJoin(Room room, RoomParticipant participant)
        {
            Members.Add(participant, room);
        }

        private void room_OnRoomMessage(object sender, Message msg)
        {
            Messages.Add(msg);
        }

        private void room_OnAdminMessage(object sender, Message msg)
        {
            Messages.Add(msg);
        }

        private void room_OnSelfMessage(object sender, Message msg)
        {
            Messages.Add(msg);
        }

        private void room_OnSubjectChange(object sender, Message msg)
        {
            SubjectAuthor = msg.From;
            Subject = msg.InnerText;
        }

        public event EventHandler<ConferenceJoinEventArgs> Joined = delegate { };
        public event EventHandler<KickedEventArgs> Kicked = delegate { };
        public event EventHandler<BannedEventArgs> Banned = delegate { };

        /// <summary>
        /// Send public message
        /// </summary>
        /// <param name="body">message body</param>
        public void SendPublicMessage(string body)
        {
            room.PublicMessage(body);
        }

        #region Implementation of INotifyPropertyChanged

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Raise a PropertyChanged event for given property
        /// </summary>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Implementation of IJabberSessionHolder

        /// <summary>
        /// Session object
        /// </summary>
        public JabberSession JabberSession { get; private set; }

        #endregion

        public override string ToString()
        {
            return ConferenceJid.ToString();
        }
    }
}