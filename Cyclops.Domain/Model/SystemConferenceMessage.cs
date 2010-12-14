using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.Model
{
    public class SystemConferenceMessage : IConferenceMessage
    {
        #region Implementation of IConferenceMessage

        /// <summary>
        /// Message author
        /// </summary>
        public string Author
        {
            get { return "System"; }
        }

        /// <summary>
        /// Message text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Message timestamp 
        /// </summary>
        public DateTime Timestamp { get { return DateTime.Now; } }
        /// <summary>
        /// False, if message is not sent by user (but probably by server\system)
        /// </summary>
        public bool IsCustom
        {
            get { return true; }
        }

        public bool IsErrorMessage { get; set; }

        #endregion
    }
}
