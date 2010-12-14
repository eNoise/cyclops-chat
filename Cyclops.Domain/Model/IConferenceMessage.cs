using System;
using jabber;

namespace Cyclops.Core.Model
{
    public interface IConferenceMessage
    {
        /// <summary>
        /// Message author
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Message text
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Message timestamp 
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// False, if message is not sent by user (but probably by server\system)
        /// </summary>
        bool IsCustom { get; }

    }
}