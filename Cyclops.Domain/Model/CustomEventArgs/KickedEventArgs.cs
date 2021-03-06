﻿using System;
using jabber;

namespace Cyclops.Core.Model.CustomEventArgs
{
    public class KickedEventArgs : EventArgs
    {
        public KickedEventArgs(JID author, string reason)
        {
            Author = author;
            Reason = reason;
        }

        public JID Author { get; private set; }
        public string Reason { get; private set; }
    }
}