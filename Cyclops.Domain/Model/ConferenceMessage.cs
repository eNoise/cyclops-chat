using System;
using jabber;
using jabber.protocol.client;

namespace Cyclops.Core.Model
{
    public class ConferenceMessage : IJabberSessionHolder, IConferenceMessage
    {
        private readonly Message msg;

        internal ConferenceMessage(JabberSession session, Message msg)
        {
            JabberSession = session;
            this.msg = msg;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Message author
        /// </summary>
        public JID AuthorJid
        {
            get { return msg.From; }
        }

        /// <summary>
        /// Message author
        /// </summary>
        public string Author
        {
            get { return msg.From.Resource; }
        }

        /// <summary>
        /// Message text
        /// </summary>
        public string Text
        {
            get { return msg.Body; }
        }

        /// <summary>
        /// Message timestamp 
        /// TODO: recieve from server
        /// </summary>
        public DateTime Timestamp { get; private set; }

        public bool IsCustom
        {
            get { return false; }
        }

        public JabberSession JabberSession { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}] {1}: {2}", Timestamp.ToShortTimeString(), Author, Text);
        }
    }
}