using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using Cyclops.Core.Model;

namespace Cyclops.Client.MessageDecoration
{
    public class CommonMessageDecorator : IMessageDecorator
    {
        #region Implementation of IMessageDecorator

        /// <summary>
        /// Transform collection of inlines
        /// </summary>
        public List<Inline> Decorate(IConferenceMessage msg, List<Inline> inlines)
        {
            inlines.Add(Decorate(msg, msg.Text));
            return inlines;
        }

        public static Inline Decorate(IConferenceMessage msg, string message)
        {
            string style = "commonMessageStyle";
            if (msg is SystemConferenceMessage)
                style = "systemMessageStyle";
            if (msg is SystemConferenceMessage && ((SystemConferenceMessage)msg).IsErrorMessage)
                style = "errorMessageStyle";

            Run messageInline = new Run(message);
            messageInline.SetResourceReference(FrameworkContentElement.StyleProperty, style);
            return messageInline;
        }

        #endregion
    }
}
